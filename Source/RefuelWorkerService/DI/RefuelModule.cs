using Ninject.Modules;
using RefuelWorkerService.Clients;
using RefuelWorkerService.Services;

namespace RefuelWorkerService.DI
{
	internal class RefuelModule : NinjectModule
	{
		public override void Load()
		{
			Bind<StationCache>().ToSelf().InSingletonScope();
			Bind<PushOverService>().ToSelf().InSingletonScope();
			Bind<RefuelHttpClient>().ToSelf().InSingletonScope();
			Bind<RefuelJsonSerializer>().ToSelf().InSingletonScope();
			Bind<SettingsCache>().ToSelf().InSingletonScope();
			Bind<SettingsLoader>().ToSelf().InSingletonScope();
			Bind<ITankerKoenigService>().To<TankerTestKoenigService>().InSingletonScope();
		}
	}
}
