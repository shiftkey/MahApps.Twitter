namespace MahApps.Twitter.Models
{
    using Newtonsoft.Json;

    public class SiteStreamsWrapper : ITwitterResponse
    {
        [JsonProperty(PropertyName = "for_user")]
        public string ForUser { get; set; }

        [JsonProperty(PropertyName = "message")]
        public SiteStreamsMessage Message { get; set; }
    }
}