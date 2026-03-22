using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text;

namespace BirdBrain.Models
{
    public class LatLng
    {
        
        public string? city_ascii { get; set; }

        public double lat { get; set; }
        
        public double lng { get; set; }
       
        public string? country { get; set; }
       
        public string? iso2 { get; set; }

        public string Display =>
        $"{city_ascii}, {country} ({lat}, {lng})";
    }
}
