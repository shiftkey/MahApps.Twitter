namespace MahApps.Twitter.Models
{
    using System.Collections.Generic;

    public class ListResult : ITwitterResponse
    {
        public List<TwitterList> Lists { get; set; }
    }
}