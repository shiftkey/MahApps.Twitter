using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MahApps.Twitter.Models
{
    public interface ITwitterResponse
    {
    }

    public class ExceptionResponse : ITwitterResponse
    {
        public String ErrorMessage { get; set; }
        public String Content { get; set; }
    }

    public class ResultsWrapper<T> : List<T>, ITwitterResponse
    {

    }
}
