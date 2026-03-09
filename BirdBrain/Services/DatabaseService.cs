using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using BirdBrain.Models;

namespace BirdBrain.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;

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
            List<BirdObservation> apiList)
        {
            var now = DateTime.UtcNow;

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
                DateStamp = now
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
    }
}