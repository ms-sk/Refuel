using Ninject.Activation.Caching;
using RefuelWorkerService.Model;

namespace RefuelWorkerService.Services
{
	internal class TankerTestKoenigService : ITankerKoenigService
	{
		static Random _rand = new Random();
		readonly StationCache cache;
		readonly TankerKoenigService tankerKoenigService;

		public TankerTestKoenigService(StationCache cache, TankerKoenigService tankerKoenigService)
		{
			this.cache = cache;
			this.tankerKoenigService = tankerKoenigService;
		}

		public Task<PriceResult?> GetPrices(List<string> stationGuids)
		{
			return Task.FromResult(new PriceResult()
			{
				Ok = true,
				Prices = CreatePrices(stationGuids)
			});
		}

		private Dictionary<string, Price> CreatePrices(List<string> stationGuids)
		{
			var dictionary = new Dictionary<string, Price>();

			foreach (var guid in stationGuids)
			{
				dictionary[guid] = new Price()
				{
					Diesel = (float)(50 * _rand.NextDouble()),
					E10 = (float)(50 * _rand.NextDouble()),
					E5 = (float)(50 * _rand.NextDouble())
				};
			}

			return dictionary;
		}

		public async Task<StationDetails> GetStationDetails(string id)
		{
			var mock = cache.Get(id);

			if(mock == null)
			{
			//	mock = await tankerKoenigService.GetStationDetails(id);
			}

			return mock;
		}
	}
}
