
using BirdBrain.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BirdBrain.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;
        public SQLiteAsyncConnection Db => _db;

        public async Task InitAsync()
        {
            if (_db != null)                                        // If No connection 
                return;

            var dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "birdbrain.db");

            _db = new SQLiteAsyncConnection(dbPath);    

            await _db.CreateTableAsync<BirdObservationDb>();        // Create the table if it doesn't exist
            await _db.ExecuteAsync(                                 // Create Indexes if not exisitng 
                @"CREATE INDEX IF NOT EXISTS idx_location_date
                    ON BirdObservationDb(Lat, Lng, DateStamp, ComName);");
        }

        public async Task SaveObservationsAsync(List<BirdObservationDb> observations)
        {
            await _db.InsertAllAsync(observations);                 //Insert into Sqlite database
        }

        public List<BirdObservationDb> ConvertToDb(
            List<BirdObservation> apiList, string refreshTime)
        {
            return apiList.Select(o => new BirdObservationDb
            {
                SpeciesCode = o.speciesCode,
                ComName = o.comName,
                SciName = o.sciName,
                LocId = o.locId,
                LocName = o.locName,
                ObsDt = o.obsDt,
                HowMany = o.howMany,
                Lat = o.lat,
                Lng = o.lng,
                ObsValid = o.obsValid,
                ObsReviewed = o.obsReviewed,
                LocationPrivate = o.locationPrivate,
                DateStamp = refreshTime,
                AppLat = App.State.SelectedSavedLocation.Lat,
                AppLng = App.State.SelectedSavedLocation.Lng
            }).ToList();
        }
    }
}