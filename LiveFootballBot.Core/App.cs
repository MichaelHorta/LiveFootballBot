using LiveFootballBot.Core.Commands;
using Microsoft.Extensions.Logging;
using System;
using Telegram.Bot.Types;

namespace LiveFootballBot.Core
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly ITelegramBotService _telegramBotService;
        private readonly ICommandManager _commandManager;

        public App(ILogger<App> logger, ITelegramBotService telegramBotService, ICommandManager commandManager)
        {
            _logger = logger;
            _telegramBotService = telegramBotService;
            _commandManager = commandManager;
        }

        public void Run()
        {
            _telegramBotService.MessageReceived += App_MessageReceived;
            _telegramBotService.StartReceiving();
        }

        public void App_MessageReceived(object sender, Message message)
        {
            Console.WriteLine($"Received message from {message.Chat.Id}. Message: {message.Text}");

            var response = _commandManager.ExceuteCommand(message.Text, message.Chat.Id);

            _telegramBotService.SendTextMessageAsync(message.Chat.Id, $"{response}");
        }
    }
}
