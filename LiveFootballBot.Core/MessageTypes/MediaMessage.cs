using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Core.MessageTypes
{
    public class MediaMessage : IMessage
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
