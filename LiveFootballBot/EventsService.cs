using LiveFootballBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LiveFootballBot
{
    public class EventsService: IEventsService
    {
        private readonly HttpClient _httpClient;

        public EventsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<EventData>> GetEvents(string apiUrl, string type = null)
        {
            RootEvents rootEvents = null;
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                rootEvents = JsonConvert.DeserializeObject<RootEvents>(content);
            }

            if (!string.IsNullOrEmpty(type))
            {
                var data = rootEvents.Data.Where(d => d.Sport != null && d.Sport.AlternateNames != null && d.Sport.AlternateNames.ContainsKey("enEN") && d.Sport.AlternateNames["enEN"].Equals(type)).ToList();
                return data;
            }
            
            return rootEvents.Data;
        }

        public async Task<EventInfo> GetEventInfo(string apiUrl)
        {
            RootEvent rootEvent = null;
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                rootEvent = JsonConvert.DeserializeObject<RootEvent>(content);
            }

            return rootEvent.Data;
        }
    }
}
