using System;
using MahApps.Twitter.Extensions;
using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class DirectMessage : ITwitterResponse
    {
        [JsonProperty(PropertyName = "sender_id")]
        public long? SenderId { get; set;}

        [JsonProperty(PropertyName =  "recipient_id")]
        public long? RecipientId { get; set;}

        public long? Id { get; set;}

        public string Text { get; set;}

        [JsonProperty(PropertyName = "sender_screen_name")]
        public string SenderScreenName { get; set; }

        [JsonProperty(PropertyName = "sender")]
        public User Sender { get; set; }

        [JsonProperty(PropertyName = "recipient_screen_name")]
        public string RecipientScreenName { get; set; }

        [JsonProperty(PropertyName = "recipient")]
        public User Recipient { get; set; }

        [JsonProperty("created_at")]
        public object CreatedDate { get; set; }

        public DateTime Created { get { return CreatedDate.ToString().ParseDateTime(); } }
    }
}
