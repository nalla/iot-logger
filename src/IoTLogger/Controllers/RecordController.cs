using IoTLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace IoTLogger.Controllers
{
	[Route( "api/[controller]" )]
	public class RecordController : Controller
	{
		private IConfiguration Configuration { get; }

		private ILogger<RecordController> Logger { get; }

		public RecordController( IConfiguration configuration, ILogger<RecordController> logger )
		{
			Logger = logger;
			Configuration = configuration;
		}

		[HttpPost( "[action]" )]
		[ProducesResponseType( 204 )]
		[ProducesResponseType( 400 )]
		public IActionResult PostAirQuality( [FromHeader] string apiKey, [FromBody] AirQualityRecording recording )
		{
			if( Configuration["apiKey"] == apiKey )
			{
				Metrics.CreateGauge( "temperature", "", "deviceid" ).Labels( recording.DeviceId ).Set( recording.Temperature );
				Metrics.CreateGauge( "humidity", "", "deviceid" ).Labels( recording.DeviceId ).Set( recording.Humidity );
				Metrics.CreateGauge( "heatindex", "", "deviceid" ).Labels( recording.DeviceId ).Set( recording.HeatIndex );
				Logger.LogInformation( "{@recording}", recording );

				return NoContent();
			}

			return BadRequest( "Invalid Api Key." );
		}
	}
}
