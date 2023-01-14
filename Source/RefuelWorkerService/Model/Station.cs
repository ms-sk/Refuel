namespace RefuelWorkerService.Model
{
	public class Station
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public int PostCode { get; set; }

        public string Place { get; set; }

        public Openingtime[] OpeningTimes { get; set; }

        public string[] Overrides { get; set; }

        public bool WholeDay { get; set; }

        public bool IsOpen { get; set; }

        public float E5 { get; set; }

        public float E10 { get; set; }

        public float Diesel { get; set; }

        public float Lat { get; set; }

        public float Ing { get; set; }

        public object State { get; set; }
    }
}

