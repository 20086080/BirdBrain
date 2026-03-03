using System.Text.Json;

namespace BirdBrain.Services
{
    public class JsonFileReader
    {
        public async Task<List<T>> ReadListAsync<T>(string fileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
