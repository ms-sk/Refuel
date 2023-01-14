using RefuelWorkerService.Configuration;

namespace RefuelWorkerService.Services
{
	public sealed class SettingsLoader
	{
		private readonly RefuelJsonSerializer jsonSerializer;

		public SettingsLoader(RefuelJsonSerializer jsonSerializer)
		{
			this.jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
		}

		public async Task<Settings?> Load(CancellationToken cancellation)
		{
			var file = await File.ReadAllTextAsync(Paths.SettingsPath, cancellation);
			return jsonSerializer.Deserialize<Settings>(file);
		}
	}
}
