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
        ///Number of Obs in location ///        
        public async Task<int> GetTotalLocationCountAsync(double lat, double lng, DateTime CutoffDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) as Sightings
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND HowMany > 0
                AND DateStamp >= ?",
                lat, lng, CutoffDate);
            return result;
        }

        ///# of Birds in location ///
        public async Task<int> GetTotalLocationBirdCountAsync(double lat, double lng, DateTime CutoffDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT COUNT(DISTINCT comName) as TypeBirds
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND HowMany > 0
                AND DateStamp >= ?",
                lat, lng, CutoffDate);
            return result;
        }

        //# Observatons by Bird in location ///
        public async Task<List<TopBirds>> GetTop5BirdCountAsync(double lat, double lng, DateTime CutoffDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            
            var result = await db.QueryAsync<TopBirds>(
                @"SELECT comName as BirdName, COUNT(*) as Sightings
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND HowMany > 0
                AND DateStamp >= ?
                GROUP BY comName
                ORDER BY Sightings DESC",
                lat, lng, CutoffDate);
            return result.ToList();
        }

        ///Daily Observation location ///
        public async Task<List<LocationDailyObs>> GetLocationDailyObsAsync(double lat, double lng, DateTime CutoffDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            
            var result = await db.QueryAsync<LocationDailyObs>(

                @"SELECT DATE(ObsDt) as ObsDt, 
                COUNT(*) as Sightings 
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND HowMany > 0
                AND DateStamp >= ? 
                GROUP BY DATE(ObsDt)",
                lat, lng, CutoffDate);
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
                        SELECT IFNULL(MAX(DateStamp),0)
                        FROM BirdObservationDb
                        WHERE AppLat = ? AND AppLng = ?
                        )",
                lat, lng, comName, lat, lng);
            return result;
        }

        ///Daily Observation Bird ///
        public async Task<List<BirdDailyObs>> GetBirdDailyObsAsync(double lat, double lng, string comName)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.QueryAsync<BirdDailyObs>(
                //HowMany capped at 100 for levelliing anomalies such as flocks > 100 etc
                @"SELECT DATE(ObsDt) as ObsDt,
                IFNULL(MIN(Sightings,100),0) as Sightings
                FROM
                (
                    SELECT DATE(ObsDt) as ObsDt,
                        SUM(HowMany) as Sightings
                    FROM BirdObservationDb b
                    WHERE AppLat = ?
                    AND AppLng = ?
                    AND ComName = ?
                    AND DateStamp = (
                        SELECT IFNULL(MAX(DateStamp),0)
                            FROM BirdObservationDb
                            WHERE AppLat = b.AppLat
                            AND AppLng = b.AppLng
                            AND ComName = b.ComName
                        )
                    GROUP BY DATE(ObsDt)
                )",
                lat, lng, comName);
            return result.ToList();
        }

        public async Task<List<BirdTimeObs>> GetBirdTimeObsAsync(double lat, double lng, string comName)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.QueryAsync<BirdTimeObs>(
                //HowMany capped at 100 for levelliing anomalies such as flocks > 100 etc
                @"SELECT DATE(ObsDt) as ObsDt,
                IFNULL(MIN(Sightings,100),0) as Sightings
                FROM
                (
                    SELECT TIME(ObsDt) as ObsDt,
                        SUM(HowMany) as Sightings
                    FROM BirdObservationDb b
                    WHERE AppLat = ?
                    AND AppLng = ?
                    AND ComName = ?
                    AND DateStamp = (
                        SELECT IFNULL(MAX(DateStamp),0)
                            FROM BirdObservationDb
                        WHERE AppLat = b.AppLat
                            AND AppLng = b.AppLng
                            AND ComName = b.ComName
                        )
                    GROUP BY TIME(ObsDt)
                    LIMIT 6
                )",
                lat, lng, comName);
            return result.ToList();
        }

        ///Has Location been Refreshed today ///        
        public async Task<bool> GetLocationRefreshTodayAsync(double lat, double lng)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var todayStart = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT Count(1) 
                FROM BirdObservationDb
                WHERE AppLat = ?
                    AND AppLng = ?
                    AND DateStamp >= ?",
                lat, lng, todayStart);
            return result>0;
        }
    }
}