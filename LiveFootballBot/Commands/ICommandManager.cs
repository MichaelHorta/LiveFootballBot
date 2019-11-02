using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Commands
{
    public interface ICommandManager
    {
        void InitializeCommands();

        string ExceuteCommand(string input);

        ICommand GetCommand(string input);
    }
}
