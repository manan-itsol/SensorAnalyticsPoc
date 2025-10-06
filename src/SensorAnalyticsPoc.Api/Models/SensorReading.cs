namespace SensorAnalyticsPoc.Api.Models
{
    public record SensorReading(string SensorId, long TimestampUtc, double Value);
}
