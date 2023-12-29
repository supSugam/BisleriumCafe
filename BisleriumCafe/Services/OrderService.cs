namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
using BisleriumCafe.Enums;
internal class OrderService(Repository<User> userRepository, Repository<Order> orderRepository, AuthService _authService)
{
    private readonly Repository<User> _userRepository = userRepository;
    private readonly Repository<Order> _orderRepository = orderRepository;

}
