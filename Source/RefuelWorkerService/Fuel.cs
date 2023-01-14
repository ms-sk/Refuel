using RefuelWorkerService.Model;

namespace RefuelWorkerService
{
	public sealed class Fuel
	{
		public decimal Price { get; set; }

		public FuelType Type { get; set; }
	}
}