using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI.Controllers
{
	///-------------------------------------------------------------------------------------------------
	/// <summary>	A controller for handling weather forecasts. </summary>
	///
	/// <seealso cref="T:Microsoft.AspNetCore.Mvc.ControllerBase"/>
	///
	[ApiController]
	[Route( "[controller]" )]
	public class WeatherForecastController : ControllerBase
	{
		/// <summary>	The summaries. </summary>
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		/// <summary>	The logger. </summary>
		private readonly ILogger<WeatherForecastController> _logger;

		///-------------------------------------------------------------------------------------------------
		/// <summary>	Constructor. </summary>
		///
		/// <param name="logger">	The logger. </param>
		///
		public WeatherForecastController( ILogger<WeatherForecastController> logger )
		{
			_logger = logger;
		}

		///-------------------------------------------------------------------------------------------------
		/// <summary>
		/// 	(An Action that handles HTTP GET requests) enumerates the items in this collection that meet given
		/// 	criteria.
		/// </summary>
		///
		/// <returns>	An enumerator that allows foreach to be used to process the matched items. </returns>
		///
		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			var rng = new Random();
			return Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays( index ),
				TemperatureC = rng.Next( -20, 55 ),
				Summary = Summaries[ rng.Next( Summaries.Length ) ]
			} )
			.ToArray();
		}
	}
}
