using LiveFootballBot.Core.MessageTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LiveFootballBot.Core
{
    public interface ITelegramBotService
    {
        event EventHandler<Message> MessageReceived;

        void StartReceiving();

        Task SendTextMessageAsync(long chatId, string text);

        void SendTextMessage(long chatId, string text);

        void SendTextMessage(long chatId, TextMessage textMessage);

        void SendTextMessages(long chatId, List<string> texts);

        void SendMediaMessage(long chatId, MediaMessage mediaMessage);

        void SendMessage(long chatId, IMessage message);
    }
}
