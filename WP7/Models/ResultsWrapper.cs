// ReSharper disable CheckNamespace
namespace MahApps.Twitter.Models
// ReSharper restore CheckNamespace
{
    using System.Collections.Generic;

    public class ResultsWrapper<T> : List<T>, ITwitterResponse
    {

    }
}