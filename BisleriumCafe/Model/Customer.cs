namespace BisleriumCafe.Model
{
    internal class Customer:User
    {
        public int TotalOrders { get; set; } = 0;
        public bool isRegularMember { get; set; }=false;
        public int FreeCoffeeCount { get; set; }=0;
        public int FreeCoffeeProgress { get; set; }=0;

        public Customer()
        {
            Role = Enums.UserRole.Customer;
        }

        public bool OrderACoffee()
        {
            TotalOrders++;
            FreeCoffeeProgress++;
            if (FreeCoffeeProgress == 10)
            {
                FreeCoffeeCount++;
                FreeCoffeeProgress = 0;
                return true;
            }

            return false;
        }

    }
}
