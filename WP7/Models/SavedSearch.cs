using System;

namespace MahApps.Twitter.Models
{
    public class SavedSearch : ITwitterResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
    }
}