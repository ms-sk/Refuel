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

		public async Task Serialize<T>(T obj)
		{
			var json = JsonSerializer.Serialize(obj);
			// TODO : entkoppeln
			await File.WriteAllTextAsync(Paths.StationCache, json);
		}
	}
}
