using IoTLogger.Models;

namespace IoTLogger.Services
{
	public interface IDataProcessor
	{
		void Process(AirQualityRecording recording);
	}
}
