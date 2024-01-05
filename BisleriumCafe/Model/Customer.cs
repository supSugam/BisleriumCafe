namespace BisleriumCafe.Model
{
    internal class Customer:User
    {
        public int TotalOrders { get; set; } = 0;
        public bool IsRegularMember { get; set; }=false;
        public int FreeCoffeeCount { get; set; }=0;
        public int FreeCoffeeProgress { get; set; }=0;

        public Customer()
        {
            Role = Enums.UserRole.Customer;
        }
    }
}
