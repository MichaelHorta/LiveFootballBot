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

        public TelegramBotService(IOptions<BotSettings> config)
        {
            _botSettings = config.Value;

            botClient = new TelegramBotClient(_botSettings.Token);

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );
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
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                await botClient.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "You said:\n" + e.Message.Text
                );
            }
        }
    }
}
