using System.Text.Json;
using System.Text.RegularExpressions;

namespace airtek
{

    public class InventoryManagementService
    {
        private readonly List<Flight> _flights;
        private Dictionary<string, Order> _orders;
        private const int MAX_BOXES_PER_FLIGHT = 20;

        public InventoryManagementService()
        {
            _flights = [];
            _orders = [];
        }

        public void LoadFlightScheduleFromFile(string filePath)
        {
            _flights.Clear();
            string[] lines;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error reading flight schedule: " + exception.Message);
                return;
            }

            Regex flightRegex = new(@"Flight (\d+): (.+) \(([A-Z]{3})\) to (.+) \(([A-Z]{3})\)");

            int currentDay = 0;

            foreach (string line in lines)
            {
                if (line.StartsWith("Day "))
                {
                    currentDay = int.Parse(line.Split(':')[0].Replace("Day ", ""));
                    continue;
                }

                Match match = flightRegex.Match(line);
                if (match.Success)
                {
                    _flights.Add(new Flight
                    {
                        Number = int.Parse(match.Groups[1].Value),
                        Departure = match.Groups[3].Value,
                        Arrival = match.Groups[5].Value,
                        Day = currentDay
                    });
                }
            }

            foreach (var flight in _flights.OrderBy(x => x.Number))
            {
                Console.WriteLine("Flight " + flight.Number + ", departure: " + flight.Departure + ", arrival " + flight.Arrival + ", day: " + flight.Day);
            }
        }

        public void LoadOrders(string jsonFilePath)
        {
            try
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                _orders = JsonSerializer.Deserialize<Dictionary<string, Order>>(jsonString);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error reading orders: " + exception.Message);

            }
        }

        public void LoadOrdersToFlights()
        {
            var orders = _orders
                .OrderBy(o => o.Key)
                .ToList();

            foreach (var order in orders)
            {
                string orderKey = order.Key;
                string destination = order.Value.Destination;

                var availableFlight = _flights
                    .Where(f => f.Arrival == destination && f.Orders.Count < MAX_BOXES_PER_FLIGHT)
                    .FirstOrDefault();

                if (availableFlight != null)
                {
                    availableFlight.Orders.Add(orderKey);
                    order.Value.FlightSchedule = availableFlight;
                }
            }
        }

        public void DisplayOrders()
        {
            foreach (var order in _orders)
            {
                Flight? flight = order.Value.FlightSchedule;

                if (flight == null)
                {
                    Console.WriteLine("order: " + order.Key + ", flightNumber: not scheduled");
                }
                else
                {
                    Console.WriteLine("order: " + order.Key + ", flightNumber " + flight.Number + ", departure: " + flight.Departure + ", arrival " + flight.Arrival + ", day: " + flight.Day);
                }
            }
        }
    }
}