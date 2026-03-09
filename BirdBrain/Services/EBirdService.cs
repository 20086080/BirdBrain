using System;
using System.Collections.Generic;
using System.Text;
using BirdBrain.Models;
using System.Net.Http;
using System.Text.Json;

namespace BirdBrain.Services
{
    public class EBirdService
    {
        private readonly HttpClient _httpClient;

        private const string BaseUrl =
            "https://api.ebird.org/v2/data/obs/geo/recent";

        public EBirdService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

            _httpClient.DefaultRequestHeaders.Add(
                "X-eBirdApiToken", "2q8bl1en148n");
        }

        public async Task<List<BirdObservation>> GetRecentObservationsAsync(
            double lat, double lng, int radiusKm, int days)
        {
            var url =
                $"{BaseUrl}?lat={lat}&lng={lng}&dist={radiusKm}&back={days}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<BirdObservation>>(
                json,  new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}