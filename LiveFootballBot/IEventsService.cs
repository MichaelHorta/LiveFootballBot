using LiveFootballBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiveFootballBot
{
    public interface IEventsService
    {
        Task<List<EventData>> GetEvents(string apiUrl, string type = null);

        Task<EventInfo> GetEventInfo(string apiUrl);
    }
}
