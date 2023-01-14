namespace RefuelWorkerService.Services
{
	public sealed class SettingsCache
	{
		private readonly SettingsLoader loader;

		public SettingsCache(SettingsLoader loader)
		{
			this.loader = loader;
		}

		public Settings? Settings { get; private set; }

		public async Task Update()
		{
			if(Settings != null)
			{
				return;
			}

			try
			{
				Settings = await loader.Load(CancellationToken.None);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}
		}
	}
}
