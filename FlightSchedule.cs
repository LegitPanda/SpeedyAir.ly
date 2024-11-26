namespace airtek
{
    public class Flight
    {
        public int Number { get; set; }
        public required string Departure { get; set; }
        public required string Arrival { get; set; }
        public int Day { get; set; }
        public List<string> Orders { get; set; } = [];
    }
}