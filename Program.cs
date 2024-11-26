
namespace airtek
{
    public class Program
    {

        public static void Main()
        {
            var inventorySystem = new InventoryManagementService();

            inventorySystem.LoadFlightScheduleFromFile("flights.txt");
            inventorySystem.LoadOrders("orders.json");
            inventorySystem.LoadOrdersToFlights();
            inventorySystem.DisplayOrders();
        }
    }
}