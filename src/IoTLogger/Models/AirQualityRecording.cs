namespace IoTLogger.Models
{
	public class AirQualityRecording
	{
		public string DeviceId { get; set; }

		public double Temperature { get; set; }

		public double Humidity { get; set; }

		public double HeatIndex { get; set; }
	}
}
