using Microsoft.Extensions.Options;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveFootballBot.Core.MessageTypes;

namespace LiveFootballBot.Core
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

            var channelId = -1001590467245;

            try
            {
                //botClient.SendPhotoAsync(channelId, "https://e00-marca.uecdn.es/assets/multimedia/imagenes/2021/08/13/16288862750358.png", "caption", Telegram.Bot.Types.Enums.ParseMode.Html);

                botClient.SendTextMessageAsync(
                  chatId: channelId,
                  text: $"{text}",
                  parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                ).GetAwaiter().GetResult();

                //botClient.SendTextMessageAsync(
                //  chatId: chatId,
                //  text: $"{text}",
                //  parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                //).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendTextMessage(long chatId, TextMessage textMessage)
        {
            if (null == textMessage || string.IsNullOrEmpty(textMessage.Text))
                return;

            var channelId = -1001590467245;

            try
            {
                //botClient.SendPhotoAsync(channelId, "https://e00-marca.uecdn.es/assets/multimedia/imagenes/2021/08/13/16288862750358.png", "caption", Telegram.Bot.Types.Enums.ParseMode.Html);

                botClient.SendTextMessageAsync(
                  chatId: channelId,
                  text: $"{textMessage.Text}",
                  parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                ).GetAwaiter().GetResult();

                //botClient.SendTextMessageAsync(
                //  chatId: chatId,
                //  text: $"{text}",
                //  parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                //).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendTextMessageAsync(long chatId, string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            var channelId = -1001590467245;

            await botClient.SendTextMessageAsync(
                  chatId: channelId,
                  text: $"{text}",
                  parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
                );

            //botClient.SendTextMessageAsync(
            //      chatId: chatId,
            //      text: $"{text}",
            //      parseMode: Telegram.Bot.Types.Enums.ParseMode.Html
            //    );
        }

        public void SendTextMessages(long chatId, List<string> texts)
        {
            foreach(var text in texts)
            {
                SendTextMessage(chatId, text);
            }
        }

        public void SendMediaMessage(long chatId, MediaMessage mediaMessage)
        {
            if (null == mediaMessage)
                return;

            var channelId = -1001590467245;

            try
            {
                botClient.SendPhotoAsync(channelId, mediaMessage.Url, "", Telegram.Bot.Types.Enums.ParseMode.Html).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(long chatId, IMessage message)
        {
            if (message is MediaMessage)
            {
                SendMediaMessage(chatId, (MediaMessage)message);
            }
            else if (message is TextMessage)
            {
                SendTextMessage(chatId, (TextMessage)message);
            }
        }
    }
}
