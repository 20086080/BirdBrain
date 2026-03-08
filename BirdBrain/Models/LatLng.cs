using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text;

namespace BirdBrain.Models
{
    public class LatLng
    {
        [JsonPropertyName("city_ascii")]
        public string CityAscii { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("iso2")]
        public string Iso2 { get; set; }

        public string Display =>
        $"{CityAscii}, {Country} ({Lat}, {Lng})";
    }
}
