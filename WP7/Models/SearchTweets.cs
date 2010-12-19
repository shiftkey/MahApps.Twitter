using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class SearchTweet : Tweet
    {
        //public string Text { get; set; }

        //[JsonProperty(PropertyName = "to_user", NullValueHandling = NullValueHandling.Ignore)]
        //public string InReplyToScreenName { get; set; }
        
        [JsonProperty(PropertyName = "from_user")]
        public string ContactName { get; set; }

        [JsonProperty(PropertyName = "profile_image_url")]
        public string ContactImage { get; set; }

        //[DefaultValue(0)]
        //[JsonProperty(PropertyName = "to_user_id", NullValueHandling = NullValueHandling.Ignore)]
        //public long? InReplyToUserId { get; set; }

        //public long? Id { get; set; }

        //public DateTime Created { get { return CreatedDate.ToString().ParseDateTime(); } }
    }
}
