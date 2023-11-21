using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coolshop_orders.Models
{
    public class Order
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 3)]
        public string ArticleName { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Range(0, int.MaxValue)]
        public decimal UnitPrice { get; set; }
        [Range(0, 100)]
        public decimal PercentageDiscount { get; set; }
        [StringLength(maximumLength: 120, MinimumLength = 3)]
        public string Buyer { get; set; }
    }
}
