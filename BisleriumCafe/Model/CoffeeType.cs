
using BisleriumCafe.Providers;

namespace BisleriumCafe.Model
{
    internal class CoffeeType:IModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CoffeeName { get; set; }
        public string CoffeeDescription { get; set; }
        public double CoffeePrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public CoffeeType(string coffeeName, string coffeeDescription, double coffeePrice)
        {
            CoffeeName = coffeeName;
            CoffeeDescription = coffeeDescription;
            CoffeePrice = coffeePrice;
        }   

        public CoffeeType StoreACoffeeType()
        {
            // store coffee type as json to file
            CoffeeType coffeeType = new CoffeeType(CoffeeName, CoffeeDescription, CoffeePrice);
            JsonFileProvider<CoffeeType> jsonFileProvider = new();
            Repository<CoffeeType> coffeeTypeRepository = new(jsonFileProvider);
            coffeeTypeRepository.Add(coffeeType);

            return coffeeType;
        }
    }
}
