using BirdBrain.Models;
using BirdBrain.Services;

public class SavedBirdService
{
    private readonly JsonFileReader _jsonReader;

    public List<Bird> Birds { get; private set; } = new();
    
    public SavedBirdService(JsonFileReader jsonReader)
    {
        _jsonReader = jsonReader;
    }

    public async Task LoadAsync()
    {
        if (Birds != null)
            return;

        Birds = await _jsonReader.ReadListAsync<Bird>("BirdsSeedData.json");
    }
}

