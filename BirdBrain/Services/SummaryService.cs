using System;
using System.Collections.Generic;
using System.Text;
using BirdBrain.Services;

namespace BirdBrain.Services
{
    public class SummaryService
    {
        private readonly DatabaseService _databaseService;

        public SummaryService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> GetTotalLocationCountAsync(double lat, double lng)
        {
            await _databaseService.InitAsync();

            var db = _databaseService.Db;

            var result = await db.ExecuteScalarAsync<int>(
                @"SELECT SUM(HowMany)
                FROM BirdObservationDb
                WHERE Lat = ?
                AND Lng = ?
                AND DateStamp = (
                SELECT MAX(DateStamp)
                FROM BirdObservationDb
                WHERE Lat = ? AND Lng = ?
                )",
                lat, lng, lat, lng);

            return result;
        }
    }
}