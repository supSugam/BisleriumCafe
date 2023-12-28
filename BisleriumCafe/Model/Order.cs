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
        public Guid CoffeeTypeId { get; set; }
        public Guid CoffeeAddInId { get; set; }
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }



    }
}
