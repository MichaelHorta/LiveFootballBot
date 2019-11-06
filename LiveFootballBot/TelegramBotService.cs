using LiveFootballBot.Commands;
using Microsoft.Extensions.Options;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Collections.Generic;

namespace LiveFootballBot
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly BotSettings _botSettings;
        private ITelegramBotClient botClient;
        public event EventHandler<Message> MessageReceived;

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

        private void OnMessageReceived(Message message)
        {
            MessageReceived?.Invoke(this, message);
        }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                OnMessageReceived(e.Message);
            }
        }

        public void SendTextMessage(long chatId, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            try
            {
                botClient.SendTextMessageAsync(
                  chatId: chatId,
                  text: $"{text}",
                  parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                ).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendTextMessageAsync(long chatId, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            botClient.SendTextMessageAsync(
                  chatId: chatId,
                  text: $"{text}",
                  parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                );
        }

        public void SendTextMessages(long chatId, List<string> texts)
        {
            foreach(var text in texts)
            {
                SendTextMessage(chatId, text);
            }
        }
    }
}
