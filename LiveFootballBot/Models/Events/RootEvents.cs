using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveFootballBot.Models.Events
{
    public class RootEvents
    {
        public string Status { get; set; }
        public List<EventData> Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class EventData
    {
        public string Id { get; set; }

        [JsonIgnore]
        public DateTime LastUpdate { get; set; }

        [JsonIgnore]
        public DateTime StartDate { get; set; }

        public Score Score { get; set; }

        public Sport Sport { get; set; }

        public SportEvent SportEvent { get; set; }

        public Tournament Tournament { get; set; }

        [JsonIgnore]
        public bool? StartDateConfirmed { get; set; }

        public EditorialInfo EditorialInfo { get; set; }

        [JsonIgnore]
        public IEnumerable<Tv> Tv { get; set; }
    }

    public class Score
    {
        public ScoreDetail AwayTeam { get; set; }
        public ScoreDetail HomeTeam { get; set; }

        [JsonIgnore]
        public Period Period { get; set; }

        [JsonIgnore]
        public Winner Winner { get; set; }
    }

    public class ScoreDetail
    {
        public string SubScore { get; set; }
        public string TotalScore { get; set; }
    }

    public class Period
    {
        public IDictionary<string, string> AlternateNames { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Winner
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Sport
    {
        public IDictionary<string, string> AlternateNames { get; set; }

        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public string Type { get; set; }
    }

    public class SportEvent
    {
        public Competitors Competitors { get; set; }

        [JsonIgnore]
        public IDictionary<string, string> AlternateNames { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public Status Status { get; set; }

        [JsonIgnore]
        public IEnumerable<string> Referees { get; set; }

        [JsonIgnore]
        public Season Season { get; set; }

        [JsonIgnore]
        public string MatchDay { get; set; }

        [JsonIgnore]
        public Phase Phase { get; set; }

        [JsonIgnore]
        public Group Group { get; set; }

        [JsonIgnore]
        public Location Location { get; set; }
    }

    public class Competitors
    {
        public Competitor AwayTeam { get; set; }
        public Competitor HomeTeam { get; set; }
    }

    public class Competitor
    {
        public string AbbName { get; set; }
        public string CommonName { get; set; }
        public string FullName { get; set; }
        public string Id { get; set; }
        public string Country { get; set; }
        //public ImageUrlSizes imageUrlSizes { get; set; }
        public string ImageUrl { get; set; }
        //public Images images { get; set; }
    }

    public class Status
    {
        public IDictionary<string, string> AlternateNames { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Season
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Phase
    {
        public IDictionary<string, string> AlternateNames { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Group
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Location
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
    }


    public class Tournament
    {
        public IDictionary<string, string> AlternateNames { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool? IsNational { get; set; }
    }

    public class EditorialInfo
    {
        public string Site { get; set; }
        public string Url { get; set; }
        public IEnumerable<OtherUrl> OtherUrls { get; set; }
    }

    public class Tv
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class OtherUrl
    {
        public string Tag { get; set; }
        public string Url { get; set; }
    }
}
