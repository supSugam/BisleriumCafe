
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

        public async Task<CoffeeType> StoreACoffeeType(this CoffeeType coffeeType)
        {
            // store coffee type as json to file
            JsonFileProvider<CoffeeType> jsonFileProvider = new();
            Repository<CoffeeType> coffeeTypeRepository = new(jsonFileProvider);
            coffeeTypeRepository.Add(coffeeType);
            await coffeeTypeRepository.FlushAsync();
            return coffeeType;
        }
    }
}
