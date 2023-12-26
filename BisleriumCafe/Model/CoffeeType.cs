

namespace BisleriumCafe.Model;
    public class CoffeeType: IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CoffeeName { get; set; }
        public string CoffeeDescription { get; set; }
        public double CoffeePrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; }

        public CoffeeType(string coffeeName, string coffeeDescription, double coffeePrice)
        {
            CoffeeName = coffeeName;
            CoffeeDescription = coffeeDescription;
            CoffeePrice = coffeePrice;
        }   

    }
