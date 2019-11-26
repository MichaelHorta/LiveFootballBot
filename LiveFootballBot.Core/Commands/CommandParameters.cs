using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Core.Commands
{
    public class CommandParameters
    {
        public static char KEY_DATE = 'd';
        public static char KEY_MATCH = 'm';
        public string Key { get; set; }
        public IDictionary<char, string> Options { get; set; }

        public CommandParameters(string key)
        {
            Key = key;
            Options = new Dictionary<char, string>();
        }
    }
}
