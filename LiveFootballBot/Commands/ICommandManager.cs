using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Commands
{
    public interface ICommandManager
    {
        void InitializeCommands();

        string ExceuteCommand(string input, long chatId);

        ICommand GetCommand(string input);
    }
}
