namespace MahApps.Twitter.Models
{
    using System;

    using Newtonsoft.Json;

    public class Hashtag
    {
        public string Text { get; set; }

        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }
}