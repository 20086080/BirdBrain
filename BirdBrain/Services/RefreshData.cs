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
            var lastRefresh =
                await App.State.Database.GetLastRefreshTimeAsync();

            if (lastRefresh.HasValue &&
                DateTime.UtcNow - lastRefresh.Value < TimeSpan.FromMinutes(60))
            {
                return false; // too soon
            }

            // API call
            var observations =
                await _ebird.GetRecentObservationsAsync(lat, lng, App.State.Radius, App.State.Days);

            var refreshTime = DateTime.UtcNow;
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