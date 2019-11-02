using LiveFootballBot.Commands;
using Microsoft.Extensions.Options;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace LiveFootballBot
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly BotSettings _botSettings;
        private ITelegramBotClient botClient;
        private readonly ICommandManager _commandManager;

        public TelegramBotService(IOptions<BotSettings> config, ICommandManager commandManager)
        {
            _botSettings = config.Value;

            botClient = new TelegramBotClient(_botSettings.Token);

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

           _commandManager = commandManager;
        }

        public void StartReceiving()
        {
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                var response = _commandManager.ExceuteCommand(e.Message.Text);
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}. Response: {response}");

                await botClient.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: $"{response}",
                  parseMode: Telegram.Bot.Types.Enums.ParseMode.Html 
                );
            }
        }
    }
}
