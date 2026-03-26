using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace BirdBrain.Models
{
    public class BirdObservationDb          // Table Name in Database BirdBrain in SQLite 
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string? SpeciesCode { get; set; }
        public string? ComName { get; set; }
        public string? SciName { get; set; }

        public string? LocId { get; set; }
        public string? LocName { get; set; }

        public string? ObsDt { get; set; }

        public int? HowMany { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

        public bool ObsValid { get; set; }

        public bool ObsReviewed { get; set; }

        public bool  LocationPrivate { get; set; }

        public string? DateStamp { get; set; }

        public double AppLat { get; set; }
        public double AppLng { get; set; }
    }
}