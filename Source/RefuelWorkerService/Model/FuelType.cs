using System.Text.Json.Serialization;

namespace RefuelWorkerService.Model
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum FuelType
    {
        E5,
        E10,
        Diesel
    }
}