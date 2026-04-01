using System;
using System.Collections.Generic;
using System.Text;
using BirdBrain.Services;

namespace BirdBrain.Services
{
    public class RefreshData
    {
        private readonly EBirdService _ebird;
        
        public RefreshData()
        {
            _ebird = new EBirdService();  
        }

        public async Task<bool> RefreshAsync(double lat, double lng)
        {
            await App.State.Database.InitAsync();
            var summaryService = new SummaryService(App.State.Database);
            //Check if refresh already done today for this location
            bool IsRefreshed = await summaryService.GetLocationRefreshTodayAsync(lat, lng);

            if (IsRefreshed)
            {
                return false;       // Refresh already done today, skip API call and DB update
            }

            // API call
            var observations =
                await _ebird.GetRecentObservationsAsync(lat, lng, App.State.MaxRadius, App.State.MinDays);     //API for 1 day only (MinDays = 1) 

            var refreshTime = DateTime.UtcNow.ToString("yyyy-MM-dd");               // Create DateStamp which is saved in Sqlite
            var dbList =
                App.State.Database.ConvertToDb(observations, refreshTime);          // Convert to Database format, including DateStamp for each record

            await App.State.Database.SaveObservationsAsync(dbList);                 // Insert records into Sqlite
            //TODO Check if this is required 
            App.State.Observations = observations;
            return true;
        }
    }
}