using LiveFootballBot.Models.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LiveFootballBot
{
    public interface IEventsService
    {
        Task<List<EventData>> GetEvents(string path, string type = null);
    }
}
