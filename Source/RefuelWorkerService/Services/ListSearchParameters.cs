using RefuelWorkerService.Model;

namespace RefuelWorkerService.Services
{
	public sealed class ListSearchParameters
	{
		public static ListSearchParameters Default { get; } = new ListSearchParameters();

		public Guid ApiKey { get; set; }

		public decimal Latitude { get; set; }

		public decimal Longitude { get; set; }

		public int Radius { get; set; }

		public string Sorting { get; set; } = "price";

		public FuelType Type { get; set; } = FuelType.E10;
	}
}
public class StationCacheRoot
{
	public string Data { get; set; }

	public string License { get; set; }

	public bool Ok { get; set; }

	public Station[] Stations { get; set; }

	public string Status { get; set; }
}