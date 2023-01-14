using Ninject.Modules;
using RefuelWorkerService.Clients;
using RefuelWorkerService.Services;

namespace RefuelWorkerService.DI
{
	internal class RefuelModule : NinjectModule
	{
		public override void Load()
		{
			Bind<EmailFactory>().ToSelf().InSingletonScope();
			Bind<RefuelHttpClient>().ToSelf().InSingletonScope();
			Bind<RefuelJsonSerializer>().ToSelf().InSingletonScope();
			Bind<SettingsCache>().ToSelf().InSingletonScope();
			Bind<SettingsLoader>().ToSelf().InSingletonScope();
			Bind<TankerKoenigService>().ToSelf().InSingletonScope();
		}
	}
}
