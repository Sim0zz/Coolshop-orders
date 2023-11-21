using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Coolshop_orders.Models;

namespace Coolshop_orders.Logic
{
    public class OrderManager
    {
        List<Order> ordersList = new List<Order>();
        // Get the exact directory where the Orders directory is located
        private string GetFileDirectory()
        {
            string directory = Environment.CurrentDirectory;
            return Directory.GetParent(directory).Parent.Parent.FullName + "\\Orders\\";
        }
        // Check if file exists
        private bool GetFile(string filePath)
        {
            if(!File.Exists(filePath))
                return false;
            else
                return true;
        }
        // Get total price of order
        private decimal GetTotal(Order order)
        {
            return order.UnitPrice * order.Quantity;
        }
        // Get total price of order with discount
        private decimal GetTotalWithDiscount(Order order)
        {
            return order.UnitPrice * order.Quantity * (1 - (order.PercentageDiscount / 100));
        }
        // Get the difference between total price with no discount and total price discounted
        private decimal GetTotalDifference(Order order)
        {
            return GetTotal(order) - GetTotalWithDiscount(order);
        }
        // Returns a string with the information of the order. Additional info can be added and will be printed at the end
        private string PrintOrder(Order order, string notes = "")
        {
            if(notes == "")
                return $"| ID {order.Id} | {order.ArticleName} | Quantità: {order.Quantity} | Prezzo unitario: {order.UnitPrice} euro | Sconto: {order.PercentageDiscount}% |";
            else
                return $"| ID {order.Id} | {order.ArticleName} | Quantità: {order.Quantity} | Prezzo unitario: {order.UnitPrice} euro | Sconto: {order.PercentageDiscount}% | {notes} |";
        }
        // Get orders from file orders.csv (to change the file name, change the OrdersFileName key in the App.config file)
        public void GetOrders()
        {
            try
            {
                string ordersFilePath = GetFileDirectory() + ConfigurationManager.AppSettings["OrdersFileName"];
                if (GetFile(ordersFilePath))
                {
                    string[] ordersFile = File.ReadAllLines(ordersFilePath);
                    foreach(string line in ordersFile)
                    {
                        string[] lineArray = line.Split(',');
                        if(
                            // Check if line is not empty
                            lineArray.Length > 0 &&
                            // Check if id is int
                            int.TryParse(lineArray[0], out int id) &&
                            // Check if article name string is of appropriate length
                            (lineArray[1].Length > 3 && lineArray[1].Length < 100) &&
                            // Check if quantity is int
                            int.TryParse(lineArray[2], out int quantity) &&
                            // Check if unit price is decimal
                            decimal.TryParse(lineArray[3], out decimal unitPrice) &&
                            // Check if percentage discount is decimal
                            decimal.TryParse(lineArray[4], out decimal percentageDiscount) &&
                            // Check if buyer string is of appropriate length
                            (lineArray[5].Length > 3 && lineArray[5].Length < 120)
                        )
                        {
                            // Add the order to the list
                            ordersList.Add(new Order
                            {
                                Id = id,
                                ArticleName = lineArray[1],
                                Quantity = quantity,
                                UnitPrice = unitPrice,
                                PercentageDiscount = percentageDiscount,
                                Buyer = lineArray[5]
                            });
                        }
                    }
                }
                else
                {
                    throw new Exception("File non trovato.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        // Prints the required informations
        public void PrintInfo()
        {
            Order highestTotal = ordersList.MaxBy(order => GetTotalWithDiscount(order));
            Order mostQuantity = ordersList.MaxBy(order => order.Quantity);
            Order highestDiscountDiff = ordersList.MaxBy(order => GetTotalDifference(order));
            Console.WriteLine("IMPORTO TOTALE PIU ALTO");
            Console.WriteLine(PrintOrder(highestTotal, $"Totale: {GetTotalWithDiscount(highestTotal)} euro"));
            Console.WriteLine();
            Console.WriteLine("ORDINE CON LA MAGGIOR QUANTITA");
            Console.WriteLine(PrintOrder(mostQuantity, $"Quantità: {mostQuantity.Quantity}"));
            Console.WriteLine();
            Console.WriteLine("MAGGIOR DIFFERENZA TRA TOTALE SENZA / CON SCONTO");
            Console.WriteLine(PrintOrder(highestDiscountDiff, $"Totale non scontato: {GetTotal(highestDiscountDiff)} euro, Totale scontato: {GetTotalWithDiscount(highestDiscountDiff)} euro"));
        }
    }
}
