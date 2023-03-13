using RefuelWorkerService.Model;

namespace RefuelWorkerService.Services
{
	public sealed class NotificationService
	{
		readonly SettingsCache cache;
		readonly ITankerKoenigService tankerKoenigService;
		readonly PushOverService pushOverService;
		readonly StationCache stationCache;

		public NotificationService(SettingsCache settingsCache, ITankerKoenigService tankerKoenigService, PushOverService pushOverService, StationCache cache)
		{
			this.cache = settingsCache ?? throw new ArgumentNullException(nameof(settingsCache));
			this.tankerKoenigService = tankerKoenigService ?? throw new ArgumentNullException(nameof(tankerKoenigService));
			this.pushOverService = pushOverService ?? throw new ArgumentNullException(nameof(pushOverService));
			stationCache = cache;
		}

		public async Task Notify(PriceResult prices)
		{
			var notifyStations = new HashSet<Stationrequest>();
			foreach (var price in prices.Prices)
			{
				foreach (var request in cache.Settings.StationRequests)
				{
					if (request.Guids.Contains(price.Key))
					{
						request.AcctepableStations.Add(price.Key);
						if (notifyStations.Contains(request))
						{
							continue;
						}
						else
						{
							notifyStations.Add(request);
						}
					}
				}
			}

			foreach (var station in notifyStations)
			{
				try
				{
					await Notify(station);
				}
				finally
				{
					station.AcctepableStations.Clear();
				}
			}
		}

		async Task Notify(Stationrequest station)
		{
			List<StationDetails> details = new();
			foreach (var item in station.AcctepableStations)
			{
				var result = await GetStationDetails(item);
				if (result != null)
				{
					details.Add(result);
				}
			}

			await pushOverService.Notify(station, details);
			await stationCache.Save();
		}

		async Task<StationDetails> GetStationDetails(string guid)
		{
			StationDetails details = stationCache.Get(guid);

			if (details == null)
			{
				details = await tankerKoenigService.GetStationDetails(guid);

				if (details == null)
				{
					return null;
				}

				stationCache.Add(details);
			}

			return details;
		}
	}
}
