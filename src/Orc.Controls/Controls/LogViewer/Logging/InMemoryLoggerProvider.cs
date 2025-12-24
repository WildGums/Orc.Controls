namespace Orc.Controls.Controls.LogViewer.Logging
{
    using Catel;
    using Microsoft.Extensions.Logging;

    internal sealed class InMemoryLoggerProvider : ILoggerProvider
    {
        private readonly ITimeProvider _timeProvider;

        public InMemoryLoggerProvider(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new InMemoryLogger(_timeProvider)
            { 
                Category = categoryName
            };
        }

        public void Dispose()
        {
            
        }
    }
}
