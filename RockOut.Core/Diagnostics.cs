using System;

namespace RockOut.Core;

public static class Diagnostics
{
    public static void LogFilterChainError(Exception ex)
    {
        Log.Error("Filter chain error", ex);
    }

    public static void LogLatencyMetric(TimeSpan latency)
    {
        Log.Info($"Audio latency: {latency.TotalMilliseconds:F2} ms");
    }

    public static void LogMajorEvent(string message)
    {
        Log.Info($"Event: {message}");
    }
}
