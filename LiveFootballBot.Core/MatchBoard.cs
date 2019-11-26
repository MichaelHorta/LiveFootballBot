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
        //private readonly List<MatchLiveItem> _liveItems;

        List<Regex> blackListRegex = new List<Regex>
        {
            new Regex(@"<\s*blockquote[^>]*>(.*?)<\s*/\s*blockquote>"),
            new Regex(@"<\s*script[^>]*>(.*?)<\s*/\s*script>"),
            new Regex(@"<\s*iframe[^>]*>(.*?)<\s*/\s*iframe>"),
            new Regex(@"<\s*figure[^>]*>(.*?)<\s*/\s*figure>"),
            new Regex(@"<\s*div[^>]*>(.*?)<\s*/\s*div>")
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
        public readonly EditorialInfo _editorialInfo;
        private readonly Competitors _competitors;

        private bool matchEnded;
        private int numberOfRequestsAfterMatchEnded = 10;

        private event Action onTimeTriggered;

        public MatchBoard(AppSettings appSettings, ITelegramBotService telegramBotService, IEventsService eventsService, string eventId, EditorialInfo editorialInfo, Competitors competitors)
        {
            _appSettings = appSettings;
            _eventsService = eventsService;
            _telegramBotService = telegramBotService;
            _eventId = eventId;
            chatsSubscribedList = new ChatSubscribedList();
            _editorialInfo = editorialInfo;
            _competitors = competitors;

            //_liveItems = new List<MatchLiveItem>();

            matchEnded = false;

            onTimeTriggered += MatchBoard_OnTimeTriggered;
            InitiateAsync(new TimeSpan(0, 0, 10));
        }

        public void SuscribeChat(long chatId)
        {
            chatsSubscribedList.Add(new ChatSubscribed()
            {
                ChatId = chatId
            }, lastComment);
        }

        private void MatchBoard_OnTimeTriggered()
        {
            if (chatsSubscribedList.Count == 0)
                return;

            var eventInfo = GetEventInfo();
            if (null == eventInfo)
                return;

            lastComment = eventInfo.Narration.Commentaries.Count;

            var commentaries = eventInfo.Narration.Commentaries;
            commentaries = Clean(commentaries);
            commentaries.Reverse();

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
            if (chatSubscribed.LastComment == 0)
            {
                //commentaries.Reverse();
                chatSubscribed.LastComment = commentaries.Count > 0 ? commentaries.Count - 1 : 0;
            }
                
            var lastCommentaries = commentaries.Take(commentaries.Count - chatSubscribed.LastComment).ToList();

            foreach (var commentary in lastCommentaries)
            {
                //\u26BD
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
