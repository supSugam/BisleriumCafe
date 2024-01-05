namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
internal class OrderService(Repository<Customer> customerRepository, Repository<Order> orderRepository, AuthService _authService)
{
    private readonly Repository<Customer> _customerRepository = customerRepository;
    private readonly Repository<Order> _orderRepository = orderRepository;
    private readonly AuthService _authService = _authService;
    private Customer? CurrentCustomer => _authService.CurrentCustomer;


    public async Task<TaskResponse> OrderACoffee(Order order)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if(CurrentCustomer is null)
        {
            response.Message = "You are not authorized to order a coffee";
            return response;
        }
        order.CustomerId = CurrentCustomer.Id;
        order.CustomerName = CurrentCustomer.FullName;
        order.CustomerUserName = CurrentCustomer.UserName;
        _orderRepository.Add(order);
        await _orderRepository.FlushAsync();
        if(order.RedeemedFreeCoffeeCount > 0)
        {
            CurrentCustomer.FreeCoffeeCount -= order.RedeemedFreeCoffeeCount;
            CurrentCustomer.FreeCoffeeProgress = 0;
        }
        else
        {
            CurrentCustomer.FreeCoffeeProgress += 1;
            if(CurrentCustomer.FreeCoffeeProgress == 10)
            {
                CurrentCustomer.FreeCoffeeCount += 1;
                CurrentCustomer.FreeCoffeeProgress = 0;
            }
        }
        bool UpdatedRepo =  _customerRepository.Update(CurrentCustomer);
        await _customerRepository.FlushAsync();
        response.IsSuccess = UpdatedRepo;
        if(!UpdatedRepo)
        {
            response.Message = "Something went wrong while updating customer details";
        }
        else
        {

        response.Message = "Order placed successfully";
        }
        return response;
    }

    public IEnumerable<Order> GetAllMyOrders()
    {
        if(CurrentCustomer is null) return new List<Order>();
        return _orderRepository.GetAll().Where(order => order.CustomerId == CurrentCustomer.Id);
    }
    public IEnumerable<Order> GetAllOrders()
    {
        return _orderRepository.GetAll();
    }

    public async Task<TaskResponse> UpdateOrderStatus(Order order, OrderStatus status) {
        TaskResponse response = new();
        response.IsSuccess = false;
        order.OrderStatus = status;
        bool UpdatedRepo = _orderRepository.Update(order);
        await _orderRepository.FlushAsync();
        response.IsSuccess = UpdatedRepo;
        if(UpdatedRepo)
        {
            response.Message = "Order status updated successfully";
        }
        else
        {
            response.Message = "Something went wrong while updating order status";

        }
        return response;
    }

    //public bool IsRegularCustomer(Customer customer)
    //{
    //    IEnumerable<DateTime> AllOrdersDatePast30Days = GetAllOrders().Where(order => order.CustomerId == customer.Id && order.OrderDate >= DateTime.Now.AddDays(-30)).Select(order=>order.OrderDate);

    //    List<DateTime> Dates = Date.GetWeekdaysList(DateTime.Now, 30);
    //    if(AllOrdersDatePast30Days.Count()< Dates.Count)
    //    {
    //        return false;
    //    }
    //    bool MeetsCondition = Dates.All(date1 => AllOrdersDatePast30Days.Where(date2 => Date.AreSameDay(date1, date2)).Count()>0);
    //    return MeetsCondition;
    //}
    public bool IsRegularCustomer(Customer customer)
    {
        // Get all orders within the last 30 days for the given customer
        var allOrdersDatePast30Days = GetAllOrders()
            .Where(order => order.CustomerId == customer.Id && order.OrderDate >= DateTime.UtcNow.AddDays(-30))
            .Select(order => order.OrderDate)
            .ToList();

        // Materialize the list of weekdays
        var weekdaysList = Date.GetWeekdaysList(DateTime.UtcNow, 30);

        // Use HashSet for faster lookup
        var orderDatesHashSet = new HashSet<DateTime>(allOrdersDatePast30Days);

        // Check if all weekdays have at least one order
        return weekdaysList.All(date => orderDatesHashSet.Contains(date));
    }

}
