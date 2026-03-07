using System;
using System.Collections.Generic;
using System.Text;

namespace BirdBrain.Models
{
    public class SavedLocation
    {
        public string Id { get; set; }
        public string? EbirdLocId { get; set; }
        public string Name { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string Description
        {
            get; set;
        }
        public string Thumbnail { get; set; }
        public string ProfileImage { get; set; }
    }
}
