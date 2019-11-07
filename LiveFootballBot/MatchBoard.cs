using LiveFootballBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LiveFootballBot
{
    public class MatchBoard
    {
        private readonly AppSettings _appSettings;
        private readonly IEventsService _eventsService;
        private readonly ITelegramBotService _telegramBotService;
        //private readonly List<MatchLiveItem> _liveItems;

        List<Regex> blackListRegex = new List<Regex>
        {
            new Regex(@"<\s*blockquote[^>]*>(.*?)<\s*/\s*blockquote>"),
            new Regex(@"<\s*script[^>]*>(.*?)<\s*/\s*script>"),
            new Regex(@"<\s*iframe[^>]*>(.*?)<\s*/\s*iframe>"),
            new Regex(@"<\s*figure[^>]*>(.*?)<\s*/\s*figure>"),
            new Regex(@"<\s*div[^>]*>(.*?)<\s*/\s*div>")
        };

        public List<ChatSubscribed> ChatsSubscribed { get; set; }
        private int lastComment;
        public readonly string _eventId;

        public string MatchName {
            get
            {
                return $"{_competitors.HomeTeam.AbbName}-{_competitors.AwayTeam.AbbName}";
            }
        }
        public readonly EditorialInfo _editorialInfo;
        private readonly Competitors _competitors;

        private bool ended;

        private event Action onTimeTriggered;

        public MatchBoard(AppSettings appSettings, ITelegramBotService telegramBotService, IEventsService eventsService, string eventId, List<ChatSubscribed> chatsSubscribed, EditorialInfo editorialInfo, Competitors competitors)
        {
            _appSettings = appSettings;
            _eventsService = eventsService;
            _telegramBotService = telegramBotService;
            _eventId = eventId;
            ChatsSubscribed = chatsSubscribed;
            _editorialInfo = editorialInfo;
            _competitors = competitors;

            //_liveItems = new List<MatchLiveItem>();

            ended = false;

            onTimeTriggered += MatchBoard_OnTimeTriggered;
            InitiateAsync(new TimeSpan(0, 0, 10));
        }

        private void MatchBoard_OnTimeTriggered()
        {
            if (ChatsSubscribed.Count == 0)
                return;

            var eventInfo = GetEventInfo();

            lastComment = eventInfo.Narration.Commentaries.Count;

            var commentaries = eventInfo.Narration.Commentaries;
            commentaries = Clean(commentaries);

            foreach (var chatSubscribed in ChatsSubscribed)
            {
                Task.Run(() => SendCommentariesToChatSuscribed(eventInfo, chatSubscribed, commentaries));
            }

            ended = eventInfo.Event.Score.Period.AlternateNames.ContainsKey("enEN") ? eventInfo.Event.Score.Period.AlternateNames["enEN"].Equals("Ended") : false;
        }

        async void InitiateAsync(TimeSpan timeSpan)
        {
            while (!ended)
            {
                onTimeTriggered?.Invoke();
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
            var commentariesProccesed = new List<Commentary>();
            foreach (var commentary in commentaries)
            {
                var text = commentary.Text;
                text = text.Replace("<strong>", string.Empty);
                text = text.Replace("</strong>", string.Empty);

                foreach (var regex in blackListRegex)
                {
                    text = regex.Replace(text, string.Empty);
                }
                commentary.Text = text;
                commentariesProccesed.Add(commentary);
            }
            return commentariesProccesed;
        }

        private void SendCommentariesToChatSuscribed(EventInfo eventInfo, ChatSubscribed chatSubscribed, List<Commentary> commentaries)
        {
            var messages = new List<string>();
            if(chatSubscribed.LastComment == 0)
                commentaries.Reverse();
            var lastCommentaries = commentaries.Take(commentaries.Count - chatSubscribed.LastComment).ToList();

            foreach (var commentary in lastCommentaries)
            {
                var message = $"<b>{_competitors.HomeTeam.AbbName} {eventInfo.Event.Score.HomeTeam.TotalScore}-{eventInfo.Event.Score.AwayTeam.TotalScore} {_competitors.AwayTeam.AbbName}</b>\n";
                if (!string.IsNullOrEmpty(commentary.MomentAction))
                    message += $"<b>{commentary.MomentAction}</b>\n";
                message += $"{commentary.Text}\n";
                messages.Add(message);
            }

            chatSubscribed.LastComment = lastComment;
            _telegramBotService.SendTextMessages(chatSubscribed.ChatId, messages);
        }
    }

    public class ChatSubscribed
    {
        public int LastComment { get; set; }
        public long ChatId { get; set; }
    }
}
