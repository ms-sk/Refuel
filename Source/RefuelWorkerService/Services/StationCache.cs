using RefuelWorkerService.Configuration;
using RefuelWorkerService.Model;

namespace RefuelWorkerService.Services
{
	public sealed class StationCache
	{
		Dictionary<string, StationDetails> _stations = new Dictionary<string, StationDetails>();
		private readonly RefuelJsonSerializer jsonSerializer;

		public StationCache(RefuelJsonSerializer jsonSerializer)
		{
			this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
		}

		public async Task Update()
		{
			try
			{
				var cacheFile = await File.ReadAllTextAsync(Paths.StationCache);

				var result = jsonSerializer.Deserialize<Dictionary<string, StationDetails>>(cacheFile);
				if (result != null)
				{
					_stations = result;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public StationDetails? Get(string guid)
		{
			try
			{
				return _stations[guid];
			}
			catch (KeyNotFoundException)
			{
				return null;
			}
		}

		public void Add(StationDetails details)
		{
			_stations[details.Station.Id] = details;
		}

		public async Task Save()
		{
			await jsonSerializer.SaveToFile(_stations, Paths.StationCache);
		}

		internal async Task Update(PriceResult prices)
		{
			foreach (var price in prices.Prices)
			{
				var station = Get(price.Key);
				if (station != null)
				{
					station.Station.Diesel = price.Value.Diesel;
					station.Station.E5 = price.Value.E5;
					station.Station.E10 = price.Value.E10;
				}
			}

			await Save();
		}
	}
}
