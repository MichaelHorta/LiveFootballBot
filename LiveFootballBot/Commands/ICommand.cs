using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Commands
{
    public interface ICommand
    {
        string Execute(CommandParameters parameters);

        bool Validate(CommandParameters parameters);

        string GetName();
    }
}
