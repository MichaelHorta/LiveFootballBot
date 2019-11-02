using LiveFootballBot.Models.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot
{
    public interface IBoard
    {
        List<EventData> SearchEventsData(string date);
    }
}
