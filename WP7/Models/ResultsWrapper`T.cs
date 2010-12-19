namespace MahApps.Twitter.Models
{
    using System.Collections.Generic;

    public class ResultsWrapper<T> : List<T>, ITwitterResponse
    {

    }
}