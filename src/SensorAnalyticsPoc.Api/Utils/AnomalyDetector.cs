namespace SensorAnalyticsPoc.Api.Utils
{
    public class AnomalyDetector
    {
        /// Assuming a very simple anomaly detection logic for demonstration purposes:
        /// i.e., if the value exceeds a certain threshold, we consider it an anomaly.
        /// 
        public static bool IsAnomalous(double value, double threshold = 98.0)
        {
            return value > threshold;
        }
    }
}
