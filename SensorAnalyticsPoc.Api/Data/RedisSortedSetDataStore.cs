using SensorAnalyticsPoc.Api.Models;
using SensorAnalyticsPoc.Api.Utils;
using StackExchange.Redis;

namespace SensorAnalyticsPoc.Api.Data
{
    public class RedisSortedSetDataStore : ISensorDataStore
    {
        private readonly IDatabase _db;
        private readonly string _sensorKey = Constants.SensorId;

        public RedisSortedSetDataStore(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task StoreReadingAsync(SensorReading reading)
        {
            double score = reading.TimestampUtc;
            await _db.SortedSetAddAsync(_sensorKey, reading.Value.ToString(), score);
        }

        public async Task StoreBatchAsync(IEnumerable<SensorReading> readings)
        {
            var batch = _db.CreateBatch();
            var tasks = new List<Task>();
            foreach (var r in readings)
            {
                double score = r.TimestampUtc;
                tasks.Add(batch.SortedSetAddAsync(_sensorKey, r.Value.ToString(), score));
            }
            batch.Execute();
            await Task.WhenAll(tasks);
        }

        public async Task PurgeOldDataAsync(TimeSpan retention)
        {
            double cutoff = DateTime.UtcNow.Add(-retention)
                .Subtract(DateTime.UnixEpoch).TotalMilliseconds;

            await _db.SortedSetRemoveRangeByScoreAsync(
                _sensorKey,
                double.NegativeInfinity,
                cutoff
            );
        }
    }
}
