using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Core.Commands
{
    public interface ICommand
    {
        string Execute(CommandParameters parameters, long chatId);

        bool Validate(CommandParameters parameters);

        string GetName();
    }
}
