using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using BirdBrain.Models;
using System.Linq;

namespace BirdBrain.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;
        public SQLiteAsyncConnection Db => _db;

        public async Task InitAsync()
        {
            if (_db != null)
                return;

            var dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "birdbrain.db");

            _db = new SQLiteAsyncConnection(dbPath);

            await _db.CreateTableAsync<BirdObservationDb>();
        }

        public async Task SaveObservationsAsync(List<BirdObservationDb> observations)
        {
            await _db.InsertAllAsync(observations);
        }

        public List<BirdObservationDb> ConvertToDb(
            List<BirdObservation> apiList, DateTime refreshTime)
        {
            //var now = DateTime.UtcNow;

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

        public async Task<List<BirdObservationDb>> GetLatestObservationsAsync()
        {
            return await _db.Table<BirdObservationDb>()
                            .OrderByDescending(x => x.DateStamp)
                            .ToListAsync();
        }

        public async Task<DateTime?> GetLastRefreshTimeAsync()
        {
            var last = await _db.Table<BirdObservationDb>()
                                .OrderByDescending(x => x.DateStamp)
                                .FirstOrDefaultAsync();

            return last?.DateStamp;
        }

        public async Task CleanupOldObservationsAsync(double lat, double lng)
        {
            // Find the two most recent DateStamp groups

            var keepDates = await _db.QueryAsync<DateTime>(
                @"SELECT DISTINCT DateStamp 
                    FROM BirdObservationDb
                    WHERE AppLat = ? AND AppLng = ?
                    ORDER BY DateStamp DESC
                    LIMIT 2",
                lat, lng);

            if (keepDates.Count < 2)
                return;


            await _db.ExecuteAsync(
                @"DELETE FROM BirdObservationDb
                    WHERE AppLat = ?
                    AND AppLng = ?
                    AND DateStamp NOT IN (?,?)",
                lat, lng, keepDates[0], keepDates[1]);
        }
    }
}