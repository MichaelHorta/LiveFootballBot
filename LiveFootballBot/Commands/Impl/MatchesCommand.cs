using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Commands
{
    public class MatchesCommand : ICommand
    {
        private readonly static int MAX_COUNT_PARAMETERS = 1;
        private readonly IBoard _board;

        public MatchesCommand(IBoard board)
        {
            _board = board;
        }

        public string Execute(CommandParameters parameters, long chatId)
        {
            var eventsData = _board.SearchEventsData(parameters.Options[CommandParameters.KEY_DATE]);

            var response = "";
            foreach (var ev in eventsData)
            {
                if (null == ev.SportEvent)
                    continue;
                response += $"<b>{ev.SportEvent.Competitors.HomeTeam.AbbName}  {ev.Score.HomeTeam.TotalScore} - {ev.Score.AwayTeam.TotalScore} {ev.SportEvent.Competitors.AwayTeam.AbbName}</b> ({ev.Tournament.Name})\n";
            }
            return response;
        }

        public string GetName()
        {
            return "/matches";
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

            return true;
        }
    }
}
