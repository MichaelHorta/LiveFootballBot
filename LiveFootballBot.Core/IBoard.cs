using LiveFootballBot.Models;
using System.Collections.Generic;

namespace LiveFootballBot.Core
{
    public interface IBoard
    {
        List<EventData> SearchEventsData(string date);

        List<MatchBoard> MatchesBoard();

        MatchBoard GetMatchBoard(string matchName);

        MatchBoard CreateMatchBoard(string matchName, string date, long chatId);
    }
}
