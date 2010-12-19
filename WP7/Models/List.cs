using System;

using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class TwitterList : ITwitterResponse
    {
        public string Mode { get; set; }

        public string Description { get; set; }

        public string Uri { get; set; }

        [JsonProperty(PropertyName = "member_count")]
        public int MemberCounter { get; set; }

        public string Slug { get; set; }

        public string Name { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "user")]
        public User Owner { get; set; }

        [JsonProperty(PropertyName = "subscriber_count")]
        public long Subscribers { get; set; }

        public long Id { get; set; }

        public bool Following { get; set; }
    }
}
