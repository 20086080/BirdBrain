using System;
using System.Collections.Generic;
using System.Text;
using BirdBrain.Services;
using BirdBrain.Models;

namespace BirdBrain.Services
{
    public class SummaryService
    {
        private readonly DatabaseService _databaseService;
        
        public SummaryService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        ///Total Observations in location ///        
        public async Task<int> GetTotalLocationCountAsync(double lat, double lng)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT SUM(HowMany) as Sightings
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND DateStamp = (
                SELECT MAX(DateStamp)
                FROM BirdObservationDb
                WHERE AppLat = ? AND AppLng = ?
                )",
                lat, lng, lat, lng);
            return result;
        }

        ///# of Birds in location ///
        public async Task<int> GetTotalLocationBirdCountAsync(double lat, double lng)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT COUNT(DISTINCT comName) as TypeBirds
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND DateStamp = (
                SELECT MAX(DateStamp)
                FROM BirdObservationDb
                WHERE AppLat = ? AND AppLng = ?)",
                lat, lng, lat, lng);
            return result;
        }

        ///Top 5 birds and # Observatons in location ///
        public async Task<List<TopBirds>> GetTop5BirdCountAsync(double lat, double lng)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.QueryAsync<TopBirds>(
                @"SELECT comName as BirdName, SUM(HowMany) as Sightings
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND DateStamp = (
                SELECT MAX(DateStamp)
                FROM BirdObservationDb
                WHERE AppLat = ? AND AppLng = ?) 
                GROUP BY comName
                ORDER BY Sightings DESC
                LIMIT 5",
                lat, lng, lat, lng);
            return result.ToList();
        }
    }
}