using IoTLogger.Models;
using Prometheus;

namespace IoTLogger.Services
{
	public class PrometheusProcessor : IDataProcessor
	{
		public void Process(AirQualityRecording recording)
		{
			Metrics.CreateGauge("temperature", "", "deviceid").Labels(recording.DeviceId).Set(recording.Temperature);
			Metrics.CreateGauge("humidity", "", "deviceid").Labels(recording.DeviceId).Set(recording.Humidity);
			Metrics.CreateGauge("heatindex", "", "deviceid").Labels(recording.DeviceId).Set(recording.HeatIndex);
		}
	}
}
