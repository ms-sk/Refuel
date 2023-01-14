namespace RefuelWorkerService.Model
{
    public class PriceResult
    {
        public bool Ok { get; set; }

        public string License { get; set; }

        public string Data { get; set; }

        public Dictionary<string, Price> Prices { get; set; }
    }

    public class Price
    {
        public string Status { get; set; }

        public decimal E5 { get; set; }

        public decimal E10 { get; set; }

        public decimal Diesel { get; set; }
    }
}