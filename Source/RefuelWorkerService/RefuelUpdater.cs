using RefuelWorkerService.Services;

namespace RefuelWorkerService
{
	public sealed class RefuelUpdater
	{
		readonly ITankerKoenigService tankerKoenigService;
		readonly SettingsCache cache;
		readonly NotificationService notificationService;
		private readonly Services.StationCache stationCache;

		public RefuelUpdater(ITankerKoenigService tankerKoenigService, SettingsCache cache, NotificationService notificationService, Services.StationCache stationCache)
		{
			this.tankerKoenigService = tankerKoenigService ?? throw new ArgumentNullException(nameof(tankerKoenigService));
			this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
			this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
			this.stationCache = stationCache ?? throw new ArgumentNullException(nameof(stationCache));
		}

		internal async Task Execute()
		{
			var guids = new HashSet<string>();
			foreach (var request in cache.Settings?.StationRequests)
			{
				foreach (var station in request.Guids)
				{
					guids.Add(station);
				}
			}
			var prices = await tankerKoenigService.GetPrices(guids.ToList());

			if (prices != null && prices.Ok)
			{
				await stationCache.Update(prices);
				await notificationService.Notify(prices);
			}
		}
	}
}