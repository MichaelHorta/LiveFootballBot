using LiveFootballBot.Models.Events;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace LiveFootballBot
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly ITelegramBotService _telegramBotService;

        public App(ILogger<App> logger, ITelegramBotService telegramBotService)
        {
            _logger = logger;
            _telegramBotService = telegramBotService;
        }

        public void Run()
        {
            _telegramBotService.StartReceiving();
        }
    }
}
