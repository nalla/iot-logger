using System;
using System.Collections.Generic;
using InfluxDB.Collector;
using IoTLogger.Models;
using Microsoft.Extensions.Configuration;

namespace IoTLogger.Services
{
	public class InfluxdbProcessor : IDataProcessor
	{
		public InfluxdbProcessor(IConfiguration configuration)
		{
			var url = configuration.GetValue<string>("influxdb:url");
			var database = configuration.GetValue<string>("influxdb:database");

			Metrics.Collector = new CollectorConfiguration()
				.Batch.AtInterval(TimeSpan.FromSeconds(2))
				.WriteTo.InfluxDB(url, database)
				.CreateCollector();
		}

		public void Process(AirQualityRecording recording)
		{
			Metrics.Collector.Write(
				"air_quality_measurement",
				new Dictionary<string, object>
				{
					{"temperature", recording.Temperature},
					{"humidity", recording.Humidity},
					{"heatindex", recording.HeatIndex}
				}, new Dictionary<string, string>
				{
					{"deviceid", recording.DeviceId}
				});
		}
	}
}
