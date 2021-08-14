using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LiveFootballBot.Models
{
    public class RootEvent
    {
        public string Status { get; set; }
        public EventInfo Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class EventInfo
    {
        public string Id { get; set; }
        public Event Event { get; set; }
        public DateTime LastUpdate { get; set; }
        public Lineup Lineup { get; set; }
        public Narration Narration { get; set; }
    }

    public class Event
    {
        public string Id { get; set; }
        [JsonIgnore]
        public DateTime LastUpdate { get; set; }

        [JsonIgnore]
        public DateTime StartDate { get; set; }

        [JsonIgnore]
        public bool StartDateConfirmed { get; set; }

        [JsonIgnore]
        public IndexInfo IndexInfo { get; set; }

        public DocType DocType { get; set; }
        public Sport Sport { get; set; }
        public Tournament Tournament { get; set; }
        public Season Season { get; set; }
        public string MatchDay { get; set; }
        public SportEvent SportEvent { get; set; }
        public Score Score { get; set; }
        public ScoreDetails ScoreDetails { get; set; }
    }

    public class IndexInfo
    {
        public string SportId { get; set; }
        public string TournamentId { get; set; }
        public DateTime EventDate { get; set; }
    }

    public class DocType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Lineup
    {
        public string Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public string EventId { get; set; }
        public Lineups Lineups { get; set; }
        public bool LineupConfirmed { get; set; }
    }

    public class Lineups
    {
        public LineupTeam AwayTeam { get; set; }
        public LineupTeam HomeTeam { get; set; }
    }

    public class LineupTeam
    {
        public string Manager { get; set; }
        public List<SubstitutesActualLineup> SubstitutesActualLineup { get; set; }
        public List<ActualLineup> ActualLineup { get; set; }
        public List<SubstitutesInicialLineup> SubstitutesInicialLineup { get; set; }
        public List<InicialLineup> InicialLineup { get; set; }
        public string FormationTeam { get; set; }
        //public TeamInfo TeamInfo { get; set; }
    }

    public class SubstitutesActualLineup
    {
        public bool Titular { get; set; }
        public string Id { get; set; }
        public string JerseyNumber { get; set; }
        public string Name { get; set; }
        public PlayerPosition PlayerPosition { get; set; }
    }

    public class ActualLineup
    {
        public bool Titular { get; set; }
        public string Id { get; set; }
        public string FormationPlace { get; set; }
        public string JerseyNumber { get; set; }
        public string Name { get; set; }
        public PlayerPosition PlayerPosition { get; set; }
    }

    public class SubstitutesInicialLineup
    {
        public string Id { get; set; }
        public string JerseyNumber { get; set; }
        public string Name { get; set; }
        public PlayerPosition PlayerPosition { get; set; }
    }

    public class InicialLineup : IComparable<InicialLineup>
    {
        public string Id { get; set; }
        public string FormationPlace { get; set; }
        public string JerseyNumber { get; set; }
        public string Name { get; set; }
        public PlayerPosition PlayerPosition { get; set; }

        public int CompareTo(InicialLineup other)
        {
            return PlayerPosition.Id - other.PlayerPosition.Id;
        }
    }

    public class PlayerPosition
    {
        public IDictionary<string, string> AlternateNames { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class Narration
    {
        public string Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime StartDate { get; set; }
        public string EventId { get; set; }
        public NarrationHead NarrationHead { get; set; }
        public List<Commentary> Commentaries { get; set; }
        public bool Blocked { get; set; }
    }

    public class ScoreDetails
    {
        public Goals Goals { get; set; }
    }

    public class Goals
    {
        public List<TeamGoal> AwayTeam { get; set; }
        public List<TeamGoal> HomeTeam { get; set; }
    }

    public class TeamGoal
    {
        public PeriodGoal Period { get; set; }
        public string _id { get; set; }
        public string PlayerFullName { get; set; }
        public string PlayerId { get; set; }
        public string PlayerCommonName { get; set; }
        public int MatchTime { get; set; }
        public DateTime Date { get; set; }
    }

    public class PeriodGoal
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public IDictionary<string, string> AlternateNames { get; set; }
    }

    public class Commentary
    {
        public bool Outstanding { get; set; }
        //public string Type { get; set; }
        public DateTime TimePublished { get; set; }
        public string MomentAction { get; set; }
        [JsonProperty(PropertyName = "commentary")]
        public string Text { get; set; }
        public List<object> Multimedia { get; set; }
        public long Id { get; set; }
        [JsonProperty(PropertyName = "content")]
        public List<CContent> CContent { get; set; }
    }

    public class CContent
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
    }

    public class NarrationHead
    {
        public string Title { get; set; }
        public Unit Unit { get; set; }
        public List<Commentator> Commentators { get; set; }
        public Language Language { get; set; }
        public NarrationType NarrationType { get; set; }
    }

    public class Unit
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Commentator
    {
        public string Name { get; set; }
        public string Rol { get; set; }
    }

    public class Language
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
    }

    public class NarrationType
    {
        public string TypeName { get; set; }
        public string TypeId { get; set; }
    }
}
