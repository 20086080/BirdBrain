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
            return result > 0;
        }

        public async Task<List<LocationSightingSummary>> GetLocationCountsAsync(double lat, double lng, string CutoffDate, string DateToday, string previousDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;

            var result = await db.QueryAsync<LocationSightingSummary>(
                @"SELECT 
                    COUNT(*) as TotalSightings,
                    COUNT(DISTINCT comName) as TotalBirds,
                    COUNT(DISTINCT DateStamp) as TotalDays,
                    COUNT(DISTINCT CASE WHEN DateStamp = ? THEN comName END) as TodayBirds,
                    COUNT(DISTINCT CASE WHEN DateStamp = ? THEN comName END) as PreviousDayBirds
                    FROM BirdObservationDb
                    WHERE AppLat = ?
                    AND AppLng = ?
                    AND HowMany > 0
                    AND DateStamp >= ?",
                DateToday, previousDate, lat, lng, CutoffDate);
            return result.ToList();
        }

        public async Task<(string latest, string previous)> GetLatestTwoDatesAsync(double lat, double lng)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;

            var result = await db.QueryAsync<DateStampResult>(
                @"SELECT DISTINCT DateStamp 
                    FROM BirdObservationDb
                    WHERE AppLat = ?
                    AND AppLng = ?
                    ORDER BY DateStamp DESC
                    LIMIT 2",
                lat, lng);

            var latest = result.ElementAtOrDefault(0)?.DateStamp;
            var previous = result.ElementAtOrDefault(1)?.DateStamp;

            return (latest, previous);
        }

        //# Observatons for each Bird in location ///
        public async Task<List<TopBirds>> GetTop5BirdCountAsync(double lat, double lng, string CutoffDate, string DateToday)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;

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
            return result.ToList();

        }

        ///Daily Observation location ///
        public async Task<List<LocationDailyObs>> GetLocationDailyObsAsync(double lat, double lng, string CutoffDate)
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
                AND Date(ObsDt) >= ? 
                GROUP BY DATE(ObsDt)
                ORDER BY DATE(ObsDt)",
                lat, lng, CutoffDate);
            return result.ToList();
        }

        ///Daily Birds Observation in location ///
        public async Task<List<BirdDailyObs>> GetBirdDailyObsAsync(double lat, double lng, string comName, string CutoffDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;

            var result = await db.QueryAsync<BirdDailyObs>(

                @"SELECT DATE(ObsDt) as ObsDt, 
                COUNT(*) as Sightings 
                FROM BirdObservationDb
                WHERE AppLat = ?
                AND AppLng = ?
                AND HowMany > 0
                AND ComName = ?
                AND Date(ObsDt) >= ? 
                GROUP BY DATE(ObsDt)
                ORDER BY DATE(ObsDt)",
                lat, lng, comName, CutoffDate);
            return result.ToList();
        }

        public async Task<List<BirdTimeObs>> GetBirdTimeObsAsync(double lat, double lng, string comName, string CutoffDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.QueryAsync<BirdTimeObs>(
                //HowMany capped at 100 for levelliing anomalies such as flocks > 100 etc
                @"SELECT ObsDt,
                IFNULL(
                    CASE WHEN Sightings > 100 THEN 100 ELSE Sightings END, 0) as Sightings
                FROM
                (
                    SELECT STRFTIME('%H:%M',ObsDt) as ObsDt,
                        SUM(HowMany) as Sightings
                    FROM BirdObservationDb b
                    WHERE AppLat = ?
                    AND AppLng = ?
                    AND HowMany > 0
                    AND ComName = ?
                    AND Date(ObsDt) >= ? 
                    GROUP BY STRFTIME('%H:%M', ObsDt)
                    ORDER BY Sightings DESC
                    LIMIT 4
                )",
                lat, lng, comName, CutoffDate);
            return result.ToList();
        }

        ///Total Observations of Bird in location ///        
        public async Task<List<BirdSightingSummary>> GetTotalBirdCountAsync(double lat, double lng, string comName, string CutoffDate)
        {
            await _databaseService.InitAsync();
            var db = _databaseService.Db;
            var result = await db.QueryAsync<BirdSightingSummary>(
                @"SELECT 
                    COUNT(*) as TotalSightings,
                    COUNT(CASE WHEN ComName = ? THEN ComName END) as BirdSightings,
                    COUNT(DISTINCT DateStamp) as TotalDays
                    FROM BirdObservationDb
                    WHERE 
                    AppLat = ?
                    AND AppLng = ?
                    AND HowMany > 0           
                    AND DateStamp >= ?",
                comName, lat, lng, CutoffDate);
            return result.ToList();
        }
        
    }
}