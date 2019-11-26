using System;

namespace LiveFootballBot.Models
{
    public class MatchLiveItem : IComparable<MatchLiveItem>
    {
        public string Icon { get; set; }
        public string Time { get; set; }

        public string _Comment;
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                Identifier = value.GetHashCode();
                _Comment = value;
            }
        }
        
        public int Identifier { get; set; }

        public int CompareTo(MatchLiveItem other)
        {
            return Identifier == other.Identifier ? 1 : 0;
        }
    }
}
