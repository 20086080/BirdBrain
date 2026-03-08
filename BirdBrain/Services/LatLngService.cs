using System;
using System.Collections.Generic;
using System.Text;
using BirdBrain.Models;
using BirdBrain.Services;

namespace BirdBrain.Services
{
    public class LatLngService
    {
        private readonly JsonFileReader _jsonReader;

        public List<LatLng> LatLng { get; private set; }

        public LatLngService(JsonFileReader jsonReader)
        {
            _jsonReader = jsonReader;
        }

        public async Task LoadAsync()
        {
            if (LatLng != null)
                return;

            LatLng = await _jsonReader.ReadListAsync<LatLng>("LatLngSeedData.json");
        }
    }
}

