using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Model
{
    internal class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public CoffeeType CoffeeType { get; set; }
        //public CoffeeAddIns coffeeAddIns { get; set; }
        

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        public Order CreateAnOrder()
        {
            return new Order
            {
                Id = Id,
                CustomerId = CustomerId,
                ProductId = ProductId,
                Quantity = Quantity,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt
            };
             }
    }
}
