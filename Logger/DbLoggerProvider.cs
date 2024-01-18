using Microsoft.Extensions.Options;

namespace ClaimManagement.Logger
{
    public class DbLoggerProvider : ILoggerProvider
    {
        public readonly DbLoggerOptions Options;
        public DbLoggerProvider(IOptions<DbLoggerOptions> Options)
        {
            this.Options = Options.Value;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(this) ;
        }

        public void Dispose()
        {
        }
    }
}
