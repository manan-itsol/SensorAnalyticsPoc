using SensorAnalyticsPoc.Api.Models;

namespace SensorAnalyticsPoc.Api.Data
{
    public interface ISensorDataStore
    {
        Task StoreReadingAsync(SensorReading reading);
        Task StoreBatchAsync(IEnumerable<SensorReading> readings);
        Task PurgeOldDataAsync(TimeSpan retention);
    }
}
