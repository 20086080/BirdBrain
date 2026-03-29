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
            bool IsRefreshed = await summaryService.GetLocationRefreshTodayAsync(App.State.SelectedSavedLocation.Lat, App.State.SelectedSavedLocation.Lng);

            if (IsRefreshed)
            {
                return false; // Refresh already done today, skip API call and DB update
            }

            // API call
            var observations =
                await _ebird.GetRecentObservationsAsync(lat, lng, App.State.MaxRadius, App.State.MinDays);     //API for 1 day only (MinDays = 1) 

            var refreshTime = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var dbList =
                App.State.Database.ConvertToDb(observations, refreshTime);

            await App.State.Database.SaveObservationsAsync(dbList);
            // Keep only last 2 refresh sets
            //await App.State.Database.CleanupOldObservationsAsync(lat, lng);

            App.State.Observations = observations;
            return true;
        }
    }
}