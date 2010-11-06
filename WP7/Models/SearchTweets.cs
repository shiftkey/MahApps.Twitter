using System;
using System.ComponentModel;
using MahApps.Twitter.Extensions;
using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class SearchTweet : Tweet
    {
        public String Text { get; set; }

        [JsonProperty(PropertyName = "to_user", NullValueHandling = NullValueHandling.Ignore)]
        public String InReplyToScreenName { get; set; }
        
        public String Source { get; set; }

        [JsonProperty(PropertyName = "from_user")]
        public String ContactName { get; set; }

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "to_user_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? InReplyToUserId { get; set; }

        public long? Id { get; set; }

        [JsonProperty("created_at")]
        public object CreatedDate { get; set;}

        public DateTime Created { get { return CreatedDate.ToString().ParseDateTime(); } }
    }
}
