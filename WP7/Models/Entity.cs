using System.Collections.Generic;
using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class Entity
    {
        [JsonProperty("user_mentions")]
        public IList<UserMention> UserMentions { get; set; }

        [JsonProperty("urls")]
        public IList<Url> Urls { get; set; }

        [JsonProperty("hashtags")]
        public IList<Hashtag> Hashtags { get; set; }
    }
}