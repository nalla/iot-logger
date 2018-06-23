using System.Collections.Generic;
using IoTLogger.Models;
using IoTLogger.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IoTLogger.Controllers
{
	[Route("api/[controller]")]
	public class RecordController : Controller
	{
		private readonly IConfiguration _configuration;
		private readonly IEnumerable<IDataProcessor> _dataProcessors;
		private readonly ILogger _logger;

		public RecordController(
			IConfiguration configuration,
			ILogger<RecordController> logger,
			IEnumerable<IDataProcessor> dataProcessors)
		{
			_logger = logger;
			_configuration = configuration;
			_dataProcessors = dataProcessors;
		}

		[HttpPost("[action]")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult PostAirQuality([FromHeader] string apiKey, [FromBody] AirQualityRecording recording)
		{
			if (_configuration["apiKey"] == apiKey)
			{
				foreach (var dataProcessor in _dataProcessors) dataProcessor.Process(recording);

				_logger.LogInformation("{@recording}", recording);

				return NoContent();
			}

			return BadRequest("Invalid Api Key.");
		}
	}
}
