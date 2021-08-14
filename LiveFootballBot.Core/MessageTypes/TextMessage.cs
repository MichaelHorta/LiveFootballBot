using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Core.MessageTypes
{
    public class TextMessage : IMessage
    {
        public string Text { get; set; }
    }
}
