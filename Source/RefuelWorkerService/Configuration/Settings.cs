
using RefuelWorkerService.Model;
using System.Text.Json.Serialization;

public class Settings
{
	public string ApiKey { get; set; }

	public string WorkerMail { get; set; }

	public string WorkerMailPassword { get; set; }

	public Stationrequest[] StationRequests { get; set; }
}

public class Stationrequest
{
	public string[] Guids { get; set; }

	public string Mail { get; set; }

	public PriceChecks[] Prices { get; set; }

	[JsonIgnore]
	public List<string> AcctepableStations { get; } = new List<string>();

}

public class PriceChecks
{
	public float Price { get; set; }

	public FuelType Type { get; set; }
}
