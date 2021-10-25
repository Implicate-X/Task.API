using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Connections;
using System.IO;

namespace TaskAPI
{
	/// <summary>	A startup. </summary>
	public class Startup
	{
		///-------------------------------------------------------------------------------------------------
		/// <summary>	Gets the logger. </summary>
		///
		/// <value>	The logger. </value>
		///
		public ILogger Logger { get; }

		///-------------------------------------------------------------------------------------------------
		/// <summary>	Gets the configuration. </summary>
		///
		/// <value>	The configuration. </value>
		///
		public IConfiguration Configuration { get; }

		///-------------------------------------------------------------------------------------------------
		/// <summary>	Gets or sets the full pathname of the base file. </summary>
		///
		/// <value>	The full pathname of the base file. </value>
		///
		public string BasePath { get; set; }

		///-------------------------------------------------------------------------------------------------
		/// <summary>	Constructor. </summary>
		///
		/// <param name="env">   	The environment. </param>
		/// <param name="logger">	The logger. </param>
		///

		public Startup( IWebHostEnvironment env, ILogger<Startup> logger )
		{
			BasePath = env.ContentRootPath;

			var builder = new ConfigurationBuilder()
			   .SetBasePath( env.ContentRootPath )
			   .AddJsonFile( "appsettings.json", optional: true, reloadOnChange: true )
			   .AddJsonFile( $"appsettings{env.EnvironmentName}.json", optional: true )
			   .AddEnvironmentVariables();

			Logger = logger;
			Configuration = builder.Build();

			Logger.LogInformation( $"appsettings{env.EnvironmentName}.json" );
		}


		///-------------------------------------------------------------------------------------------------
		/// <summary>	Configure services. </summary>
		///
		/// <param name="services">	The services. </param>
		///
		public void ConfigureServices( IServiceCollection services )
		{
			services.AddSignalR();

			services.AddCors( setupAction: options =>
			{
				options.AddPolicy( "CorsPolicy",
					builder =>
						builder.AllowAnyOrigin()
							   .AllowAnyMethod()
							   .AllowAnyHeader()
							   .AllowCredentials()
							   .WithOrigins( "http://localhost:8080" ) );
			} );

			services.AddControllers();
			services.AddMvc()
				.SetCompatibilityVersion( CompatibilityVersion.Latest );

			// Implementation of the OpenAPI-GUI
			// 
			services.AddSwaggerGen( options =>
			 {
				 string xmlDocPath = Configuration[ "WebAPi:XmlDocPath:FileName" ];

				 var apiInfo = new OpenApiInfo
				 {
					 Title = Configuration[ "WebApi:Title" ],
					 Version = Configuration[ "WebApi:Version" ],
					 Description = Configuration[ "WebApi:Description" ],
					 TermsOfService = new Uri( Configuration[ "WebApi:License:Terms" ].ToString() ),
					 Contact = new OpenApiContact
					 {
						 Name = Configuration[ "WebApi:Contact:Name" ],
						 Email = Configuration[ "WebApi:Contact:Mail" ],
						 Url = new Uri( Configuration[ "WebApi:Contact:Url" ] )
					 },
					 License = new OpenApiLicense
					 {
						 Name = Configuration[ "WebApi:License:Copyright" ],
						 Url = new Uri( Configuration[ "WebApi:License:Terms" ] )
					 }
				 };

				 string filePath = Path.Combine( BasePath, xmlDocPath );

				 options.IncludeXmlComments( filePath, true );
				 options.UseInlineDefinitionsForEnums();

				 options.SwaggerDoc( "controllers", apiInfo );
			 } );

			services.AddHttpContextAccessor();
			services.AddScoped<HttpContextAccessor>();

			services.AddHttpClient();
			services.AddScoped<HttpClient>();

			services.AddSingleton( Configuration );
		}

		///-------------------------------------------------------------------------------------------------
		/// <summary>	Configures. </summary>
		///
		/// <param name="app">				The application. </param>
		/// <param name="env">				The environment. </param>
		/// <param name="loggerFactory">	The logger factory. </param>
		///
		public void Configure( IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory )
		{
			if ( env.IsDevelopment() )
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			// 
			app.UseSwagger();
			app.UseSwaggerUI( options =>
			 {
				 options.DocExpansion( DocExpansion.None );

				 options.SwaggerEndpoint( "/swagger/controllers/swagger.json", "REST API" );
			 } );

			app.UseCors( "CorsPolicy" );

			app.UseEndpoints( endpoints =>
			{
				endpoints.MapControllers();

				endpoints.MapSwagger();
			} );

			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.Use( async ( context, next ) =>
			 {
				 if ( !context.Request.Path.Value.Contains( "/swagger", StringComparison.OrdinalIgnoreCase ) )
				 {
					 context.Response.Redirect( "swagger" );
					 return;
				 }

				 await next();
			 } );
		}
	}
}
