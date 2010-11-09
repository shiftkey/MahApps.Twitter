using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MahApps.Twitter.Extensions;
using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class Tweet : ITwitterResponse
    {
        [JsonProperty(PropertyName = "retweeted_status")]
        public Tweet RetweetedStatus { get; set; }

        public String Text { get; set; }

        [JsonProperty(PropertyName = "in_reply_to_screen_name", NullValueHandling = NullValueHandling.Ignore)]
        public String InReplyToScreenName { get; set; }

        [JsonProperty(PropertyName = "favorited")]
        public bool Favourited { get; set; }

        public String Source { get; set; }

        public User User { get; set; }

        public Geo Geo { get; set; }

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "in_reply_to_status_id", NullValueHandling =  NullValueHandling.Ignore)]
        public long? InReplyToStatusId { get; set; }

        [DefaultValue(0)]
        [JsonProperty(PropertyName = "in_reply_to_user_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? InReplyToUserId { get; set; }

        public bool Truncated { get; set; }

        public Geo Coordinates { get; set; }

        public long? Id { get; set; }

        [JsonProperty("created_at")]
        public object CreatedDate { get; set;}

        public DateTime Created { get { return CreatedDate.ToString().ParseDateTime(); } }
    }
}
