// ReSharper disable CheckNamespace
namespace MahApps.Twitter.Models
// ReSharper restore CheckNamespace
{
    using Newtonsoft.Json;

    public class DirectMessageContainer
    {
        [JsonProperty(PropertyName = "direct_message")]
        public DirectMessage DirectMessage { get; set; }
    }
}