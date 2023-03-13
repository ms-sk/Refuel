using RefuelWorkerService.Configuration;
using System.Text.Json;

namespace RefuelWorkerService.Services
{
	public sealed class RefuelJsonSerializer
	{
		JsonSerializerOptions options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
		};

		public T? Deserialize<T>(string json)
		{
			return JsonSerializer.Deserialize<T>(json, options);
		}

		public async Task SaveToFile<T>(T obj, string path)
		{
			var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
			await File.WriteAllTextAsync(path, json);
		}
	}
}
