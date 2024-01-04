namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
using BisleriumCafe.Enums;
internal class OrderService(Repository<User> userRepository, Repository<Order> orderRepository, AuthService _authService)
{
    private readonly Repository<User> _userRepository = userRepository;
    private readonly Repository<Order> _orderRepository = orderRepository;
    private readonly Customer? CurrentCustomer = _authService.CurrentCustomer; 


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
        await _userRepository.FlushAsync();
        response.IsSuccess = true;
        response.Message = "Order placed successfully";
        return response;
    }

    public IEnumerable<Order> GetAllMyOrders()
    {
        if(CurrentCustomer is null) return new List<Order>();
        return _orderRepository.GetAll().Where(order => order.CustomerId == CurrentCustomer.Id);
    }

}
