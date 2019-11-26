using LiveFootballBot.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace LiveFootballBot.Core
{
    public class Board : IBoard
    {
        private readonly AppSettings _appSettings;
        private readonly IEventsService _eventsService;
        private readonly ITelegramBotService _telegramBotService;

        /// <summary>
        /// Events information
        /// </summary>
        IDictionary<string, List<EventData>> EventsDictionary;

        /// <summary>
        /// List of board of matches
        /// </summary>
        private readonly List<MatchBoard> _matchesBoard;

        public Board(IOptions<AppSettings> config, ITelegramBotService telegramBotService)
        {
            _appSettings = config.Value;
            _telegramBotService = telegramBotService;

            EventsDictionary = new Dictionary<string, List<EventData>>();
            _eventsService = new EventsService();
            var now = DateTime.Now;
            var date = now.ToString("yyyy-MM-dd");
            GetEventsData(date);

            _matchesBoard = new List<MatchBoard>();
        }

        public List<EventData> SearchEventsData(string date)
        {
            List<EventData> eventsData;
            EventsDictionary.TryGetValue(date, out eventsData);
            if (null == eventsData)
            {
                eventsData = GetEventsData(date);
            }
            return eventsData;
        }

        private List<EventData> GetEventsData(string date)
        {
            var eventsData = _eventsService.GetEvents($"{_appSettings.UnidadeditorialAPI.Events}&date={date}", "Football").GetAwaiter().GetResult();
            EventsDictionary.Add(date, eventsData);
            return eventsData;
        }

        public List<MatchBoard> MatchesBoard()
        {
            return _matchesBoard;
        }

        public MatchBoard GetMatchBoard(string matchName)
        {
            return _matchesBoard.Find(o => o.MatchName == matchName);
        }

        public MatchBoard CreateMatchBoard(string matchName, string date, long chatId)
        {
            var events = SearchEventsData(date);
            var match = events.Find(ev => matchName.Equals($"{ev.SportEvent.Competitors.HomeTeam.AbbName}-{ev.SportEvent.Competitors.AwayTeam.AbbName}"));

            if (null == match)
                return null;

            var chatsSubscribed = new List<ChatSubscribed>()
                {
                    new ChatSubscribed
                    {
                        ChatId = chatId
                    }
                };
            var matchBoard = new MatchBoard(_appSettings, _telegramBotService, _eventsService, match.Id, match.EditorialInfo, match.SportEvent.Competitors);
            matchBoard.SuscribeChat(chatId);
            _matchesBoard.Add(matchBoard);

            return matchBoard;
        }
    }
}
