using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Model
{
    internal class Order: IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerUserName { get; set; }
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
