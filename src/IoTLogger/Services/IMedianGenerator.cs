using IoTLogger.Models;

namespace IoTLogger.Services
{
	public interface IMedianGenerator
	{
		bool HasMedian { get; }

		void AddRecording(AirQualityRecording recording);

		(double temperature, double humidity, double heatIndex) GetMedian();
	}
}
