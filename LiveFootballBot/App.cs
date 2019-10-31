using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LiveFootballBot
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly AppSettings _config;

        public App(ILogger<App> logger, IOptions<AppSettings> config)
        {
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            
        }

        
    }
}
