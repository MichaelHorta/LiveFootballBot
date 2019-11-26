using System;
using System.Text.RegularExpressions;

namespace LiveFootballBot.Core.Commands
{
    public class FollowMatchCommand : ICommand
    {
        private readonly static int MAX_COUNT_PARAMETERS = 2;
        private readonly IBoard _board;

        public FollowMatchCommand(IBoard board)
        {
            _board = board;
        }

        public string Execute(CommandParameters parameters, long chatId)
        {
            MatchBoard matchBoard = null;
            string matchName = null;
            string date = null;
            try
            {
                matchName = parameters.Options[CommandParameters.KEY_MATCH];
                date = parameters.Options[CommandParameters.KEY_DATE];
                matchBoard = _board.GetMatchBoard(matchName);
                if (null == matchBoard)
                    matchBoard = _board.CreateMatchBoard(matchName, date, chatId);
                else
                    matchBoard.SuscribeChat(chatId);
            }
            catch (Exception)
            {
                return null;
            }

            return null == matchBoard ? $"Match {matchName} not found" : $"You started to follow match {matchName}";
        }

        public string GetName()
        {
            return "/follow";
        }

        public bool Validate(CommandParameters parameters)
        {
            var options = parameters.Options;
            int cantParametrosEspecificados = options.Count;
            if (cantParametrosEspecificados != MAX_COUNT_PARAMETERS)
                return false;

            if (!options.ContainsKey(CommandParameters.KEY_DATE))
                return false;

            if (!DateTime.TryParse(options[CommandParameters.KEY_DATE], out DateTime dDate))
            {
                return false;
            }

            if (!options.ContainsKey(CommandParameters.KEY_MATCH))
                return false;

            Regex regex = new Regex(@"[A\--Z]");
            var match = regex.Match(options[CommandParameters.KEY_MATCH]);
            if (!match.Success)
                return false;

            return true;
        }
    }
}
