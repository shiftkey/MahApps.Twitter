namespace MahApps.Twitter.Models
{
    public class Geo : ITwitterResponse
    {
        public string Type { get; set; }

        public double[] Coordinates { get; set; }

        public double? Lat
        {
            get
            {
                if (Coordinates != null)
                    return Coordinates[0];

                return null;
            }
        }
        public double? Long
        {
            get
            {
                if (Coordinates != null)
                    return Coordinates[1];

                return null;
            }
        }
    }
}
