using System.Collections.Concurrent;
using System.Linq;
using IoTLogger.Models;

namespace IoTLogger.Services
{
	public class MedianGenerator : IMedianGenerator
	{
		private readonly ConcurrentDictionary<string, AirQualityRecording> lastRecordings = new ConcurrentDictionary<string, AirQualityRecording>();

		public bool HasMedian => !lastRecordings.IsEmpty;

		public void AddRecording(AirQualityRecording recording)
		{
			lastRecordings.AddOrUpdate(recording.DeviceId, recording, (x, y) => recording);
		}

		public (double temperature, double humidity, double heatIndex) GetMedian()
		{
			if (HasMedian)
				return (lastRecordings.Values.Sum(x => x.Temperature) / lastRecordings.Count,
					lastRecordings.Values.Sum(x => x.Humidity) / lastRecordings.Count,
					lastRecordings.Values.Sum(x => x.HeatIndex) / lastRecordings.Count);

			return default;
		}
	}
}
