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

        ///Top  birds and # Observatons in location ///
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
                ORDER BY Sightings DESC",
                lat, lng, lat, lng);
            return result.ToList();
        }

        ///Daily Observation location ///
        public async Task<List<LocationDailyObs>> GetLocationDailyObsAsync(double lat, double lng)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.QueryAsync<LocationDailyObs>(
                @"SELECT DATE(ObsDt) as ObsDt, SUM(HowMany) as Sightings
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND DateStamp = (
                SELECT MAX(DateStamp)
                FROM BirdObservationDb
                WHERE AppLat = ? AND AppLng = ?
                )
                GROUP BY DATE(ObsDt)
                ORDER BY DATE(ObsDt)",
                lat, lng, lat, lng);
            return result.ToList();
        }

        ///Total Observations of Bird in location ///        
        public async Task<int> GetTotalBirdCountAsync(double lat, double lng, string comName)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT SUM(HowMany) as Sightings
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND ComName = ?
                AND DateStamp = (
                SELECT MAX(DateStamp)
                FROM BirdObservationDb
                WHERE AppLat = ? AND AppLng = ?
                )",
                lat, lng, comName, lat, lng);
            return result;
        }
    }
}