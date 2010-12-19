using System.Collections.Generic;

namespace MahApps.Twitter.Models
{
    public class ResultsWrapper : ITwitterResponse
    {
        public List<SearchTweet> Results { get; set; }
    }
}