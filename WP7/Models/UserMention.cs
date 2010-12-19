using Newtonsoft.Json;
  
namespace MahApps.Twitter.Models
{
    public class UserMention
    {
        public string Id { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        public string Name { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }
}