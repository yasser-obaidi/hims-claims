

using System.Diagnostics.CodeAnalysis;

namespace ClaimManagement.Logger
{
    public class DbLogger : ILogger
    {
        private readonly DbLoggerProvider _dbLoggerProvider;

        public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider)
        {
                _dbLoggerProvider = dbLoggerProvider; 
        }
        public IDisposable? BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
           return logLevel > LogLevel.Information;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            //var options = new DbContextOptionsBuilder<Context>();
            //    options.UseMySQL(_dbLoggerProvider.Options.ConnectionString);
            //    using Context _funddbContext = new Context(options.Options);   // we can use a other data base just for logging 
            //    var logMessage = new SysLog();
            //    logMessage.LogDetails = $"{logLevel.ToString()} --${eventId.Name}-- ${formatter(state, exception)}";
            //    _funddbContext.SysLogs.Add(logMessage);
            //    _funddbContext.SaveChanges();
            return;
            
        }
    }
}
