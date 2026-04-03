using System;
using System.Collections.Generic;
using System.Text;

namespace BirdBrain.Models
{
    public class BirdObservationLatest
    {
        public DateTime? ObsDt { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }
}
