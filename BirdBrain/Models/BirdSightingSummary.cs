using System;
using System.Collections.Generic;
using System.Text;

namespace BirdBrain.Models
{
    public class BirdSightingSummary
    {
        public int TotalSightings { get; set; }

        public int BirdSightings { get; set; }
        public int TotalDays { get; set; }

        //public int TodayBirds { get; set; }

        //public int PreviousDayBirds { get; set; }
    }
}
