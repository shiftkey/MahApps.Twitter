namespace MahApps.Twitter.Models
{
    using System.Collections.Generic;

    public class ListResult : ITwitterResponse
    {
        public IList<TwitterList> Lists { get; set; }
    }
}