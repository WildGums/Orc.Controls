namespace Orc.Controls.Controls.LogViewer.Logging
{
    using System;
    using System.Collections.Generic;
    using Catel;
    using Microsoft.Extensions.Logging;

    internal class InMemoryLogger : ILogger
    {
        private readonly ITimeProvider _timeProvider;

        private readonly List<LogEntry> _logEntries = new List<LogEntry>();

        public InMemoryLogger(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public required string Category { get; set; }

        public int MaxCount { get; set; } = 1000;

        public IReadOnlyList<LogEntry> LogEntries => _logEntries.AsReadOnly();

        public IDisposable? BeginScope<TState>(TState state) 
            where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logEntries.Add(new LogEntry
            {
                DateTime = _timeProvider.GetUtcNow(),
                Category = Category,
                LogLevel = logLevel,
                Message = formatter(state, exception)
            });
        }
    }
}
