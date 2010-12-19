using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class Hashtag
    {
        public string Text { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }
}