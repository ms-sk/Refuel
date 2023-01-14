using RefuelWorkerService.Clients;
using RefuelWorkerService.Model;
using System.Text.Json;
using System.Web;

namespace RefuelWorkerService.Services
{
	public sealed class TankerKoenigService
	{
		readonly RefuelHttpClient httpClient;
		private readonly RefuelJsonSerializer serializer;
		private readonly SettingsCache cache;

		public TankerKoenigService(RefuelHttpClient httpClient, RefuelJsonSerializer serializer, SettingsCache cache)
		{
			this.httpClient = httpClient;
			this.serializer = serializer;
			this.cache = cache;
		}


		public async Task<PriceResult?> GetPrices(List<string> stationGuids)
		{
			var result = await httpClient.Send(httpClient.CreateMessage(BuildPricesUri(stationGuids)));

			var json = await result.Content.ReadAsStringAsync();

			return serializer.Deserialize<PriceResult>(json);
		}

		public async Task<StationDetails> GetStationDetails(string id)
		{
			var result = await httpClient.Send(httpClient.CreateMessage(BuildDetailsUri(id)));

			var json = await result.Content.ReadAsStringAsync();

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			var details = JsonSerializer.Deserialize<StationDetails?>(json, options);
			return details;
		}

		string BuildPricesUri(List<string> stationGuids)
		{
			var builder = new UriBuilder("https://creativecommons.tankerkoenig.de/json/prices.php");

			var query = HttpUtility.ParseQueryString(builder.Query);

			query["ids"] = string.Join(",", stationGuids.Select(guid => guid.ToString()));
			query["apikey"] = cache.Settings?.ApiKey ?? throw new InvalidOperationException();

			builder.Query = query.ToString();

			return builder.ToString();
		}

		string BuildDetailsUri(string id)
		{
			var builder = new UriBuilder("https://creativecommons.tankerkoenig.de/json/detail.php");

			var query = HttpUtility.ParseQueryString(builder.Query);

			query["id"] = id.ToString();
			query["apikey"] = cache.Settings?.ApiKey ?? throw new InvalidOperationException();

			builder.Query = query.ToString();

			return builder.ToString();
		}
	}
}
