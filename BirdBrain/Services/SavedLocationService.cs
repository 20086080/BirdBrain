using BirdBrain.Models;
using BirdBrain.Services;

public class SavedLocationService
{
    private readonly JsonFileReader _jsonReader;

    public List<SavedLocation> Locations { get; private set; } = new();

    public SavedLocationService(JsonFileReader jsonReader)
    {
        _jsonReader = jsonReader;
    }

    public async Task LoadAsync()
    {
        if (Locations != null)
            return;

        Locations = await _jsonReader.ReadListAsync<SavedLocation>("LocationSeedData.json");
    }
}
