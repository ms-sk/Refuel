using RefuelWorkerService.Model;

namespace RefuelWorkerService.Services
{
	public interface ITankerKoenigService
	{
		Task<StationDetails> GetStationDetails(string id);

		Task<PriceResult?> GetPrices(List<string> stationGuids);
	}
}