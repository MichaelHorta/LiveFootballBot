using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace LiveFootballBot.Core
{
    public interface ITelegramBotService
    {
        event EventHandler<Message> MessageReceived;

        void StartReceiving();

        void SendTextMessageAsync(long chatId, string text);

        void SendTextMessage(long chatId, string text);

        void SendTextMessages(long chatId, List<string> texts);
    }
}
