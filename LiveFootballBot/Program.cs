using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using LiveFootballBot.Core;

namespace LiveFootballBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = ConfigureServices();

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            // entry to run app
            serviceProvider.GetService<App>().Run();

            Thread.Sleep(int.MaxValue);
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            // add logging
            serviceCollection.AddSingleton(new LoggerFactory()
              .AddConsole()
              .AddDebug());
            serviceCollection.AddLogging();

            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("botsettings.json", false)
                .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));
            serviceCollection.Configure<BotSettings>(configuration.GetSection("Configuration"));

            // add services
            serviceCollection.AddSingleton<ITelegramBotService, TelegramBotService>();
            serviceCollection.AddSingleton<Core.Commands.ICommandManager, Core.Commands.CommandManager>();
            serviceCollection.AddSingleton<IBoard, Board>();

            // add app
            serviceCollection.AddTransient<App>();

            return serviceCollection;
        }
    }
}
