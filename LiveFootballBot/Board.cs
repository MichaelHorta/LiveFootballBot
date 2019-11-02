using LiveFootballBot.Models.Events;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace LiveFootballBot
{
    public class Board : IBoard
    {
        private readonly AppSettings _appSettings;
        private readonly IEventsService _eventsService;
        public IDictionary<string, List<EventData>> EventsDictionary { get; set; }

        public Board(IOptions<AppSettings> config)
        {
            _appSettings = config.Value;

            EventsDictionary = new Dictionary<string, List<EventData>>();
            _eventsService = new EventsService();
            var now = DateTime.Now;
            var date = now.ToString("yyyy-MM-dd");
            GetEventsData(date);
        }

        public List<EventData> SearchEventsData(string date)
        {
            List<EventData> eventsData;
            EventsDictionary.TryGetValue(date, out eventsData);
            if(null == eventsData)
            {
                eventsData = GetEventsData(date);
            }
            return eventsData;
        }

        private List<EventData> GetEventsData(string date)
        {
            var eventsData = _eventsService.GetEvents(string.Format($"{_appSettings.Unidadeditorial.Api}&date={date}"), "Football").GetAwaiter().GetResult();
            EventsDictionary.Add(date, eventsData);
            return eventsData;
        }
    }
}
