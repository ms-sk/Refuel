using RefuelWorkerService.Model;

namespace RefuelWorkerService.Services
{
	public sealed class NotificationService
	{
		readonly SettingsCache cache;
		readonly TankerKoenigService tankerKoenigService;
		readonly EmailFactory factory;
		readonly StationCache stationCache;

		public NotificationService(SettingsCache settingsCache, TankerKoenigService tankerKoenigService, EmailFactory factory, StationCache cache)
		{
			this.cache = settingsCache ?? throw new ArgumentNullException(nameof(settingsCache));
			this.tankerKoenigService = tankerKoenigService ?? throw new ArgumentNullException(nameof(tankerKoenigService));
			this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
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
				details.Add(await GetStationDetails(item));
			}

			var email = factory.Create(details, station.Mail);

			factory.Send(new SendParameter
			{
				Mail = cache.Settings.WorkerMail,
				Password = cache.Settings.WorkerMailPassword,
				TargetMail = station.Mail,
				Content = email
			});

			await stationCache.Save();
		}

		async Task<StationDetails> GetStationDetails(string guid)
		{
			StationDetails details = stationCache.Get(guid);

			if (details == null)
			{
				details = await tankerKoenigService.GetStationDetails(guid);
				stationCache.Add(details);
			}

			return details;
		}
	}
}
