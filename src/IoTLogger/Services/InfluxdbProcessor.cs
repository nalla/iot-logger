using System;
using System.Collections.Generic;
using InfluxDB.Collector;
using IoTLogger.Models;
using Microsoft.Extensions.Configuration;

namespace IoTLogger.Services
{
	public class InfluxdbProcessor : IDataProcessor
	{
		private readonly IMedianGenerator medianGenerator;

		public InfluxdbProcessor(IConfiguration configuration, IMedianGenerator medianGenerator)
		{
			var url = configuration.GetValue<string>("influxdb:url");
			var database = configuration.GetValue<string>("influxdb:database");

			this.medianGenerator = medianGenerator;
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
					{ "temperature", recording.Temperature },
					{ "humidity", recording.Humidity },
					{ "heatindex", recording.HeatIndex }
				}, new Dictionary<string, string>
				{
					{ "deviceid", recording.DeviceId }
				});

			medianGenerator.AddRecording(recording);

			if (medianGenerator.HasMedian)
			{
				var (temperature, humidtiy, heatIndex) = medianGenerator.GetMedian();

				Metrics.Collector.Write(
					"median_air_quality_measurement",
					new Dictionary<string, object>
					{
						{ "temperature", temperature },
						{ "humidity", humidtiy },
						{ "heatindex", heatIndex }
					});
			}
		}
	}
}
