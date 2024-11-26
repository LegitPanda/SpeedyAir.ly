using System.Text.Json.Serialization;

namespace airtek
{
    public class Order
    {
        [JsonPropertyName("destination")]
        public required string Destination { get; set; }

        public Flight? FlightSchedule { get; set; }
    }
}