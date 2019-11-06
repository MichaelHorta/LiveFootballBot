using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Commands
{
    public class StartCommand : ICommand
    {
        public string Execute(CommandParameters parameters, long chatId)
        {
            return "Hello Start";
        }

        public bool Validate(CommandParameters parameters)
        {
            return true;
        }

        public string GetName()
        {
            return "/start";
        }
    }
}
