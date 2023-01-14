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
			var cacheFile = await File.ReadAllTextAsync(Paths.StationCache);

			var result = jsonSerializer.Deserialize<Dictionary<string, StationDetails>>(cacheFile);
			if (result != null)
			{
				_stations = result;
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
			await jsonSerializer.Serialize(_stations);
		}
	}
}
