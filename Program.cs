using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TaskAPI
{
	/// <summary> A program. </summary>
	public class Program
	{
		///-------------------------------------------------------------------------------------------------
		/// <summary> Main entry-point for this application. </summary>
		///
		/// <param name="args"> The arguments. </param>
		///
		public static void Main( string[] args )
		{
			var host = CreateWebHostBuilder( args ).Build();


			host.Run();
		}

		///-------------------------------------------------------------------------------------------------
		/// <summary> Creates web host builder. </summary>
		///
		/// <param name="args"> The arguments. </param>
		///
		/// <returns> The new web host builder. </returns>
		///
		public static IWebHostBuilder CreateWebHostBuilder( string[] args ) =>
			WebHost.CreateDefaultBuilder( args )
				   .ConfigureLogging( ( hostingContext, builder ) =>
				   {
					   builder.AddConfiguration( hostingContext.Configuration.GetSection( "Logging" ) );
					   builder.AddConsole();
					   builder.AddDebug();
				   } )
				   .UseKestrel()
				   .UseContentRoot( Directory.GetCurrentDirectory() )
				   .UseIIS()
				   .UseIISIntegration()
				   .UseStartup<Startup>();
	}
}
