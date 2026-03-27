using BirdBrain.Models;
using BirdBrain.Services;
using LiveChartsCore.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
        public async Task<int> GetTotalLocationCountAsync(double lat, double lng, DateTime CompareDate)
        {
            string CutoffDate = CompareDate.ToString("yyyy-MM-dd");
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) as Sightings
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND HowMany > 0
                AND Date(ObsDt) >= ?",
                lat, lng, CutoffDate);
            return result;
        }

        ///Total # of Birds in location ///
        public async Task<int> GetTotalLocationBirdCountAsync(double lat, double lng, DateTime CompareDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            string CutoffDate = CompareDate.ToString("yyyy-MM-dd");
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT COUNT(DISTINCT comName) as TypeBirds
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND HowMany > 0
                AND Date(ObsDt) >= ?",
                lat, lng, CutoffDate);
            return result;
        }

        //# Observatons for each Bird in location ///
        public async Task<List<TopBirds>> GetTop5BirdCountAsync(double lat, double lng, DateTime CompareDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            string CutoffDate = CompareDate.ToString("yyyy-MM-dd");
            var DateToday = await db.ExecuteScalarAsync<string>(
                                    @"SELECT MAX(DateStamp) FROM BirdObservationDb
                                        WHERE AppLat = ?
                                        AND AppLng = ?",
                                        lat, lng);
            
            var result = await db.QueryAsync<TopBirds>(
                @"SELECT comName as BirdName, 
                    IFNULL(SUM(CASE WHEN DateStamp = ? THEN 1 ELSE 0 END),0) as StatsToday,
                    COUNT(*) as Sightings
                    FROM BirdObservationDb
                        WHERE AppLat = ?
                        AND AppLng = ?
                        AND HowMany > 0
                        AND DateStamp >= ?
                GROUP BY comName
                ORDER BY Sightings DESC",
                DateToday, lat, lng, CutoffDate);
            Console.WriteLine("Hello World!");
            return result.ToList();

        }

        ///Daily Observation location ///
        public async Task<List<LocationDailyObs>> GetLocationDailyObsAsync(double lat, double lng, DateTime CompareDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            string CutoffDate = CompareDate.ToString("yyyy-MM-dd");
            var result = await db.QueryAsync<LocationDailyObs>(

                @"SELECT DATE(ObsDt) as ObsDt, 
                COUNT(*) as Sightings 
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND HowMany > 0
                AND Date(ObsDt) >= ? 
                GROUP BY DATE(ObsDt)
                ORDER BY DATE(ObsDt)",
                lat, lng, CutoffDate);
            return result.ToList();
        }




        ///Total Observations of Bird in location ///        
        public async Task<int> GetTotalBirdCountAsync(double lat, double lng, string comName, DateTime CutoffDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT SUM(HowMany) as Sightings
                FROM BirdObservationDb
                WHERE AppLat = ?
                    AND AppLng = ?
                    AND ComName = ?
                    AND HowMany > 0
                    AND Date(ObsDt) >= ?)",
                lat, lng, comName, CutoffDate);
            return result;
        }

        ///Daily Observation Bird ///
        public async Task<List<BirdDailyObs>> GetBirdDailyObsAsync(double lat, double lng, string comName, DateTime CutoffDate)
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
                    AND HowMany > 0
                    AND ComName = ?
                    AND Date(ObsDt) >= ?
                    GROUP BY DATE(ObsDt))",
                lat, lng, comName, CutoffDate);
            return result.ToList();
        }

        public async Task<List<BirdTimeObs>> GetBirdTimeObsAsync(double lat, double lng, string comName, DateTime CutoffDate)
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
                    AND HowMany > 0
                    AND ComName = ?
                    AND Date(ObsDt) >= ? 
                    GROUP BY TIME(ObsDt)
                    LIMIT 6
                )",
                lat, lng, comName, CutoffDate);
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