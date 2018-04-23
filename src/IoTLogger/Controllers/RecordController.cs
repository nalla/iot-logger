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

		[HttpPost]
		[ProducesResponseType( 204 )]
		[ProducesResponseType( 400 )]
		public IActionResult Post( [FromHeader] string apiKey, [FromBody] Recording recording )
		{
			if( Configuration["apiKey"] == apiKey )
			{
				Metrics.CreateGauge( recording.Name, "", nameof(recording.Location) ).Labels( recording.Location ).Set( recording.Value );
				Logger.LogInformation( "{@recording}", recording );

				return NoContent();
			}

			return BadRequest( "Invalid Api Key." );
		}
	}
}
