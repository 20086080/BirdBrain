using System.Text.Json;
using System.Diagnostics;

namespace BirdBrain.Services
{
    public class JsonFileReader
    {
        public async Task<List<T>> ReadListAsync<T>(string fileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
           
            try
            {
           
                return await JsonSerializer.DeserializeAsync<List<T>>(stream);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
