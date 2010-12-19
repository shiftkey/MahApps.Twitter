using System;
using MahApps.Twitter.Extensions;
using Newtonsoft.Json;

namespace MahApps.Twitter.Models
{
    public class User : ITwitterResponse
    {
        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty(PropertyName = "show_all_inline_media")]
        public bool ShowAllInlineMedia { get; set; }

        [JsonProperty(PropertyName = "contributors_enabled")]
        public bool ContributorsEnabled { get; set; }

        [JsonProperty(PropertyName = "profile_background_tile")]
        public bool ProfileBackgroundTile { get; set; }

        [JsonProperty(PropertyName = "profile_sidebar_border_color")]
        public string ProfileSidebarBorderColour { get; set; }

        [JsonProperty(PropertyName = "profile_use_background_image")]
        public bool ProfileUseBackgroundimage { get; set; }

        [JsonProperty(PropertyName = "profile_background_color")]
        public string ProfileBackgroundColour { get; set; }

        [JsonProperty(PropertyName = "profile_sidebar_fill_color")]
        public string ProfileSidebarFillColour { get; set; }

        [JsonProperty(PropertyName = "profile_link_color")]
        public string ProfileLinkColour { get; set; }

        [JsonProperty(PropertyName = "profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty(PropertyName = "profile_background_image_url")]
        public string ProfileBackgroundImageUrl { get; set; }

        [JsonProperty(PropertyName = "profile_text_color")]
        public string ProfileTextColour { get; set; }

        public string Lang { get; set; }

        [JsonProperty(PropertyName = "followers_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? FollowersCount { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public bool Verified { get; set; }

        [JsonProperty(PropertyName = "friends_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? FriendsCount { get; set; }

        [JsonProperty(PropertyName = "geo_enabled")]
        public bool GeoEnabled { get; set; }

        [JsonProperty(PropertyName = "favourites_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? FavouritesCount { get; set; }

        public bool Protected { get; set; }

        public string Url { get; set; }
        
        public string Name { get; set; }

        [JsonProperty(PropertyName = "listed_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? ListedCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty(PropertyName = "statuses_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? StatusCount { get; set; }

        [JsonProperty(PropertyName = "utc_offset", NullValueHandling = NullValueHandling.Ignore)]
        public long? UTCOffset { get; set; }

        // TODO: should this be a different type?
        [JsonProperty("created_at")]
        public object CreatedDate { get; set; }

        public DateTime Created { get { return CreatedDate.ToString().ParseDateTime(); } }
    }
}
