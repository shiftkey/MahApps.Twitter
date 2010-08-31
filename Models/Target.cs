using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class StreamEvent : ITwitterResponse
    {
        [JsonProperty(PropertyName = "target")]
        public User TargetUser { get; set; }

        [JsonProperty(PropertyName = "source")]
        public User SourceUser { get; set; }
        
        [JsonProperty(PropertyName = "target_object")]
        public Tweet Target { get; set; }

        public String Event { get; set; }
    }
}
