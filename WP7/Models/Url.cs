using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class Url
    {
        [JsonProperty("indices")]
        public int[] Indices { get; set; }

        [JsonProperty("url")]
        public string Link { get; set; }

        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        [JsonProperty("expanded_url")]
        public string ExpandedUrl { get; set; }
    }
}