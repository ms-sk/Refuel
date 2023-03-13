using Ninject;
using RefuelWorkerService.DI;
using RefuelWorkerService.Services;

namespace RefuelWorkerService
{
	public partial class Worker : BackgroundService
	{
		readonly ILogger<Worker> _logger;
		readonly StandardKernel _kernel = new(new RefuelModule());
		readonly RefuelUpdater _updater;
		readonly TimeSpan _updateInterval = new(0, 0, 10);

		public Worker(ILogger<Worker> logger)
		{
			_logger = logger;
			_updater = _kernel.Get<RefuelUpdater>();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var settingsCache = _kernel.Get<SettingsCache>();
			await settingsCache.Update();

			var stationCache = _kernel.Get<Services.StationCache>();
			await stationCache.Update();

			while (!stoppingToken.IsCancellationRequested)
			{
				await _updater.Execute();
				await Task.Delay(_updateInterval, stoppingToken);
			}
		}
	}
}