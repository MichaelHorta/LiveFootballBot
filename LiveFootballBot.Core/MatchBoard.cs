using LiveFootballBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LiveFootballBot.Core
{
    public class MatchBoard
    {
        private readonly AppSettings _appSettings;
        private readonly IEventsService _eventsService;
        private readonly ITelegramBotService _telegramBotService;

        List<Regex> blackListRegex = new List<Regex>
        {
            new Regex(@"<\s*blockquote[^>]*>(.*?)<\s*/\s*blockquote>"),
            new Regex(@"<\s*script[^>]*>(.*?)<\s*/\s*script>"),
            new Regex(@"<\s*iframe[^>]*>(.*?)<\s*/\s*iframe>"),
            new Regex(@"<\s*figure[^>]*>(.*?)<\s*/\s*figure>"),
            new Regex(@"<\s*div[^>]*>(.*?)<\s*/\s*div>"),
            new Regex(@"<\s*p[^>]*>(.*?)<\s*/\s*p>")
        };

        private ChatSubscribedList chatsSubscribedList;
        private int lastComment;
        public readonly string _eventId;

        public string MatchName {
            get
            {
                return $"{_competitors.HomeTeam.AbbName}-{_competitors.AwayTeam.AbbName}";
            }
        }
        private readonly Competitors _competitors;
        private Lineup lineup;

        private bool matchEnded;
        private int numberOfRequestsAfterMatchEnded = 10;

        private event Action onTimeTriggered;

        public MatchBoard(AppSettings appSettings, ITelegramBotService telegramBotService, IEventsService eventsService, string eventId, Competitors competitors)
        {
            _appSettings = appSettings;
            _eventsService = eventsService;
            _telegramBotService = telegramBotService;
            _eventId = eventId;
            chatsSubscribedList = new ChatSubscribedList();
            _competitors = competitors;

            matchEnded = false;

            onTimeTriggered += MatchBoard_OnTimeTriggered;
            InitiateAsync(new TimeSpan(0, 0, 30));
        }

        public MatchBoard(AppSettings appSettings, ITelegramBotService telegramBotService, IEventsService eventsService, string eventId)
        {
            _appSettings = appSettings;
            _eventsService = eventsService;
            _telegramBotService = telegramBotService;
            _eventId = eventId;
        }

        public void SuscribeChat(long chatId)
        {
            var chat = chatsSubscribedList.FirstOrDefault(ch => ch.ChatId == chatId);
            if (null != chat)
                return;

            chatsSubscribedList.Add(new ChatSubscribed()
            {
                ChatId = chatId
            }, lastComment);
        }

        private void MatchBoard_OnTimeTriggered()
        {
            //if (chatsSubscribedList.Count == 0)
            //    return;

            var eventInfo = GetEventInfo();
            if (null == eventInfo)
                return;

            if (null == eventInfo.Narration.Commentaries)
                return;

            if (null == lineup && null != eventInfo.Lineup)
                lineup = eventInfo.Lineup;

            InicialLineup();

            //lastComment = eventInfo.Narration.Commentaries.Count;

            var commentaries = eventInfo.Narration.Commentaries;
            commentaries = Clean(commentaries);

            lastComment = commentaries.Count;

            foreach (var chatSubscribed in chatsSubscribedList)
            {
                Task.Run(() => SendCommentariesToChatSuscribed(eventInfo, chatSubscribed, commentaries));
            }

            matchEnded = eventInfo.Event.Score.Period.AlternateNames.ContainsKey("enEN") ? eventInfo.Event.Score.Period.AlternateNames["enEN"].Equals("Ended") : false;
        }

        async void InitiateAsync(TimeSpan timeSpan)
        {
            while (!matchEnded || numberOfRequestsAfterMatchEnded > 0)
            {
               onTimeTriggered?.Invoke();
                if (matchEnded)
                    numberOfRequestsAfterMatchEnded--;
                await Task.Delay(timeSpan);
            }
        }

        private EventInfo GetEventInfo()
        {
            var apiURL = $"{ _appSettings.UnidadeditorialAPI.Event.Replace("{ID}", _eventId)}";

            return _eventsService.GetEventInfo(apiURL).GetAwaiter().GetResult();
        }

        private List<Commentary> Clean(List<Commentary> commentaries)
        {
            var xx = commentaries.Where(o => o.CContent.Where(oo => oo != null && oo.Type.Contains("paragraph")).Count() > 1).ToList();
            if (xx.Count() > 1)
            {
                Console.WriteLine("asd");
            }
            var commentariesProccesed = new List<Commentary>();
            foreach (var commentary in commentaries)
            {
                //if (null != commentary.CContent)
                //    continue;

                if (commentary.CContent.Count() > 1)
                {
                    Console.WriteLine("asd");
                }

                if (xx.Where(o => o.Id.Equals(commentary.Id)).Count() > 0)
                {
                    Console.WriteLine("asd");
                }

                //Imagen [0] con caption [1] y creditos [2]
                if (commentary.CContent.Count == 3 && commentary.CContent[0].Type.Equals("image-reference") && commentary.CContent[1].Type.Equals("paragraph") && commentary.CContent[2].Type.Equals("paragraph"))
                {
                    continue;
                }

                //Texto [0] con imagen[1]
                if (commentary.CContent.Count == 3 && commentary.CContent[0].Type.Equals("paragraph") && commentary.CContent[1].Type.Equals("image-reference") && commentary.CContent[2].Type.Equals("paragraph"))
                {
                    continue;
                }

                for (var i = 0; i < commentary.CContent.Count; i++)
                {
                    var ccontent = commentary.CContent[i];

                    if (!ccontent.Type.Equals("paragraph"))
                        continue;

                    var content = ccontent.Content;
                    content = content.Replace("<strong>", string.Empty);
                    content = content.Replace("</strong>", string.Empty);

                    foreach (var regex in blackListRegex)
                    {
                        content = regex.Replace(content, string.Empty);
                    }
                    commentary.CContent[i].Content = content;
                    commentariesProccesed.Add(commentary);
                }
            }
            return commentariesProccesed;
        }

        private void SendCommentariesToChatSuscribed(EventInfo eventInfo, ChatSubscribed chatSubscribed, List<Commentary> commentaries)
        {
            var messages = new List<string>();
            if (chatSubscribed.LastComment == 0)
            {
                chatSubscribed.LastComment = commentaries.Count > 0 ? commentaries.Count - 1 : 0;
            }

            var lastCommentaries = commentaries.Take(commentaries.Count - chatSubscribed.LastComment).ToList();
            lastCommentaries.Reverse();

            foreach (var commentary in lastCommentaries)
            {
                //\u26BD
                var message = $"<b>{_competitors.HomeTeam.AbbName} {eventInfo.Event.Score.HomeTeam.TotalScore}-{eventInfo.Event.Score.AwayTeam.TotalScore} {_competitors.AwayTeam.AbbName}</b>\n";
                if (!string.IsNullOrEmpty(commentary.MomentAction))
                    message += $"<b>{commentary.MomentAction}</b>\n";

                foreach(var ccontent in commentary.CContent)
                {
                    message += $"{ccontent.Content.Trim()}\n";
                }
                
                messages.Add(message);
            }

            chatSubscribed.LastComment = lastComment;
            _telegramBotService.SendTextMessages(chatSubscribed.ChatId, messages);
        }

        private string InicialLineup()
        {
            //HomeTeam
            var inicialLineupStr = $"{_competitors.HomeTeam.FullName}: ";
            var homeTeamInicialLineup = lineup.Lineups.HomeTeam.InicialLineup;
            homeTeamInicialLineup.Sort();
            var homeTeamNames = homeTeamInicialLineup.Select(p => p.Name);
            inicialLineupStr += $"{string.Join(", ", homeTeamNames)}";

            inicialLineupStr += $"\n\n";

            //AwayTeam
            inicialLineupStr += $"{_competitors.AwayTeam.FullName}: ";
            var awayTeamInicialLineup = lineup.Lineups.AwayTeam.InicialLineup;
            awayTeamInicialLineup.Sort();
            var awayTeamNames = awayTeamInicialLineup.Select(p => p.Name);
            inicialLineupStr += $"{string.Join(", ", awayTeamNames)}";

            return inicialLineupStr;
        }
    }

    public class ChatSubscribedList : List<ChatSubscribed>
    {
        public void Add(ChatSubscribed chatSubscribed, int lastComment)
        {
            chatSubscribed.LastComment = lastComment;
            Add(chatSubscribed);
        }
    }

    public class ChatSubscribed
    {
        public int LastComment { get; set; }
        public long ChatId { get; set; }
    }
}
