using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MahApps.Twitter.Models
{
    public class Geo : ITwitterResponse
    {
        public String Type { get; set; }

        public Double[] Coordinates { get; set; }
    }
}
