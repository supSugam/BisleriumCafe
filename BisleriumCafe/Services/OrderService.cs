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

        response.IsSuccess = true;
        response.Message = "Order placed successfully";
        return response;
    }

}
