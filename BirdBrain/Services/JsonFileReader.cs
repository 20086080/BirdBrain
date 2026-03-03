using System.Text.Json;
using System.Diagnostics;

namespace BirdBrain.Services
{
    public class JsonFileReader
    {
        public async Task<List<T>> ReadListAsync<T>(string fileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            try
            {
                return JsonSerializer.Deserialize<List<T>>(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
