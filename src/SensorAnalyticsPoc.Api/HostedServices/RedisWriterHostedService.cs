using SensorAnalyticsPoc.Api.Data;
using SensorAnalyticsPoc.Api.Models;
using System.Threading.Channels;

namespace SensorAnalyticsPoc.Api.HostedServices
{
    public class RedisWriterHostedService : BackgroundService
    {
        private readonly Channel<List<SensorReading>> _channel;
        private readonly ISensorDataStore _store;
        private readonly List<SensorReading> _buffer = new();
        private const int BatchSize = 1000;

        public RedisWriterHostedService(Channel<List<SensorReading>> channel, ISensorDataStore store)
        {
            _channel = channel;
            _store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await foreach (var readings in _channel.Reader.ReadAllAsync(stoppingToken))
                {
                    _buffer.AddRange(readings);
                    if (_buffer.Count >= BatchSize)
                    {
                        await _store.StoreBatchAsync(_buffer);
                        _buffer.Clear();
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
