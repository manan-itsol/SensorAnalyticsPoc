using Microsoft.AspNetCore.SignalR;
using SensorAnalyticsPoc.Api.Hubs;
using SensorAnalyticsPoc.Api.Models;
using SensorAnalyticsPoc.Api.Utils;
using System.Threading.Channels;

namespace SensorAnalyticsPoc.Api.HostedServices
{
    public class SensorSimulatorService : BackgroundService
    {
        private readonly IHubContext<SensorHub> _hubContext;
        private readonly Channel<List<SensorReading>> _channel;
        private readonly Random _rand = new();

        public SensorSimulatorService(IHubContext<SensorHub> hubContext, Channel<List<SensorReading>> channel)
        {
            _hubContext = hubContext;
            _channel = channel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var batch = new List<SensorReading>(100);
                var anomalies = new List<SensorReading>();
                for (int i = 0; i < 100; i++)
                {
                    var reading = new SensorReading(
                        SensorId: Constants.SensorId,
                        TimestampUtc: DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                        Value: Math.Round(_rand.NextDouble() * 100, 2)
                    );


                    // Check for anomalies
                    if (AnomalyDetector.IsAnomalous(reading.Value))
                    {
                        anomalies.Add(reading);
                    }
                    batch.Add(reading);
                }
                // Write to channel for further processing (e.g., storage)
                await _channel.Writer.WriteAsync(batch, stoppingToken);

                // Push batch to all clients
                await _hubContext.Clients.All.SendAsync("SensorBatch", batch, cancellationToken: stoppingToken);
                await _hubContext.Clients.All.SendAsync("AnomalyAlert", anomalies, cancellationToken: stoppingToken);

                /// Send in batches of 100 readings every 100ms to acheive 1000 readings/sec
                /// This is to simulate high-frequency data generation and smooth transitions on the client side.
                /// 100 readings * 10 times/sec = 1000/sec
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
