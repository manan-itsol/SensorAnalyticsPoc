using SensorAnalyticsPoc.Api.Data;
using SensorAnalyticsPoc.Api.Utils;

namespace SensorAnalyticsPoc.Api.HostedServices
{
    public class RedisPurgerHostedService : BackgroundService
    {
        private readonly ISensorDataStore _store;
        private readonly TimeSpan _retention = TimeSpan.FromHours(Constants.DataRetentionHours);

        public RedisPurgerHostedService(ISensorDataStore store)
        {
            _store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _store.PurgeOldDataAsync(_retention);
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}
