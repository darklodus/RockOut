using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;

namespace RockOut.Core;

public static class Log
{
    private static ILogger? _logger;
    public static void Init(string logFilePath = "rockout.log", LogEventLevel level = LogEventLevel.Information)
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Is(level)
            .WriteTo.Console()
            .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();
        _logger.Information("Logging initialized");
    }
    public static void Info(string message) => _logger?.Information(message);
    public static void Warn(string message) => _logger?.Warning(message);
    public static void Error(string message, Exception? ex = null) => _logger?.Error(ex, message);
    public static void Event(string message) => _logger?.Information("EVENT: {Event}", message);
    public static void Dispose() => (_logger as IDisposable)?.Dispose();
}
