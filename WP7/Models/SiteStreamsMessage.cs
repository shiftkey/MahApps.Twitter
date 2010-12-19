namespace MahApps.Twitter.Models
{
    using Newtonsoft.Json;

    public class SiteStreamsMessage : Tweet
    {
        [JsonProperty(PropertyName = "direct_message")]
        public DirectMessage DirectMessage { get; set; }
    }
}