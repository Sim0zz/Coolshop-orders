using Coolshop_orders.Logic;

namespace Coolshop_orders
{
    public class Program
    {
        static void Main(string[] args)
        {
            //string directory = Environment.CurrentDirectory;
            //Console.WriteLine(Directory.GetParent(directory).Parent.Parent.FullName+"\\Orders");
            OrderManager orderManager = new OrderManager();
            orderManager.GetOrders();
            orderManager.PrintInfo();
        }
    }
}
