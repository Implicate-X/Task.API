using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Task.API;

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
			CGlobal m_global = new CGlobal();

			//@Robert: In dem Controler habe ich die Methode SetGlobal(CGlobal global) geschrieben.
			//Wie kann ich diese Methode aus Main aufrufen? ... bzw die Instanz der Klasse CGlobal 'm_global' an den Controller übergeben?
			//Die Controller-Instanz wird bei jedem HTTP(s) request ja neu erstellt...

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
