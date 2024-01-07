using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Model
{
    public class Order: IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? CustomerId { get; set; }
        public string? CustomerName { get; set; } = "Customer";
        public string CustomerUserName { get; set; } = "Normal-Customer";
        public CoffeeType CoffeeType { get; set; }

        public List<CoffeeAddIn> CoffeeAddIns { get; set; }
        public int Quantity { get; set; }
        public int RedeemedFreeCoffeeCount { get; set; }=0;

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Total { get; set; }
        public decimal ServiceCharge { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public Order()
        {
        }


    }
}
