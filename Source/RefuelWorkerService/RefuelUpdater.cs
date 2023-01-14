using RefuelWorkerService.Services;

namespace RefuelWorkerService
{
	public sealed class RefuelUpdater
	{
		readonly TankerKoenigService tankerKoenigService;
		readonly SettingsCache cache;
		readonly NotificationService notificationService;

		public RefuelUpdater(TankerKoenigService tankerKoenigService, SettingsCache cache, NotificationService notificationService)
		{
			this.tankerKoenigService = tankerKoenigService ?? throw new ArgumentNullException(nameof(tankerKoenigService));
			this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
			this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
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
				await notificationService.Notify(prices);
			}
		}
	}
}