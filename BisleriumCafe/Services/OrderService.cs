namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
internal class OrderService(Warehouse<Member> memberWarehouse, Warehouse<Order> orderWarehouse, AuthService _authService)
{
    private readonly Warehouse<Member> _memberWarehouse = memberWarehouse;
    private readonly Warehouse<Order> _orderWarehouse = orderWarehouse;
    private readonly AuthService _authService = _authService;
    private Member? CurrentMember => _authService.CurrentMember;
    public async Task<TaskResponse> OrderACoffee(Order order,bool IsMemberOrder)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if(IsMemberOrder && CurrentMember is null)
        {
            response.Message = "You are not authorized to order a coffee";
            return response;
        }

        if (IsMemberOrder && CurrentMember is not null) {
            order.CustomerId = CurrentMember.Id;
            order.CustomerName = CurrentMember.FullName;
            order.CustomerUserName = CurrentMember.UserName;
        }

        _orderWarehouse.Add(order);
        await _orderWarehouse.FlushAsync();
        if(IsMemberOrder && CurrentMember is not null)
        {
            if (order.RedeemedFreeCoffeeCount > 0)
            {
                CurrentMember.FreeCoffeeCount -= order.RedeemedFreeCoffeeCount;
                CurrentMember.FreeCoffeeProgress = 0;
            }
            else
            {
                CurrentMember.FreeCoffeeProgress += 1;
                if (CurrentMember.FreeCoffeeProgress == 10)
                {
                    CurrentMember.FreeCoffeeCount += 1;
                    CurrentMember.FreeCoffeeProgress = 0;
                }
            }
            CurrentMember.TotalOrders += 1;
        bool UpdatedRepo = _memberWarehouse.Update(CurrentMember);
            response.IsSuccess = UpdatedRepo;

        }
        else
        {
            response.IsSuccess = true;
        }


        await _memberWarehouse.FlushAsync();
        if(!response.IsSuccess)
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
        if(CurrentMember is null) return new List<Order>();
        return _orderWarehouse.GetAll().Where(order => order.CustomerId == CurrentMember.Id);
    }
    public IEnumerable<Order> GetAllOrders()
    {
        return _orderWarehouse.GetAll();
    }

    public async Task<TaskResponse> UpdateOrderStatus(Order order, OrderStatus status) {
        TaskResponse response = new();
        response.IsSuccess = false;
        order.OrderStatus = status;
        bool UpdatedRepo = _orderWarehouse.Update(order);
        await _orderWarehouse.FlushAsync();
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

    public IEnumerable<Order> GetOrdersWithInTimeRange(DateTime start, DateTime end)
    {
        return _orderWarehouse.GetAll().Where(order => order.OrderDate >= start && order.OrderDate <= end);
    }

    public IEnumerable<CoffeeType> GetTopCoffeeTypesWithInTimeRange(DateTime start, DateTime end,int LIMIT=5)
    {
        IEnumerable<Order> allOrders = GetOrdersWithInTimeRange(start, end);

        var coffeeTypeCounts = allOrders
        .GroupBy(order => order.CoffeeType)
        .Select(group => new
        {
            CoffeeType = group.Key,
            Count = group.Count()
        });

        var topCoffeeTypes = coffeeTypeCounts
            .OrderByDescending(coffeeTypeCount => coffeeTypeCount.Count)
            .Take(LIMIT)
            .Select(coffeeTypeCount => coffeeTypeCount.CoffeeType);
        return topCoffeeTypes;
    }

    public IEnumerable<CoffeeAddIn> GetTopCoffeeAddInsWithInTimeRange(DateTime start, DateTime end, int LIMIT = 5)
    {
        IEnumerable<Order> allOrders = GetOrdersWithInTimeRange(start, end);

        var coffeeAddIns = allOrders
            .SelectMany(order => order.CoffeeAddIns)
            .GroupBy(addIn => addIn.Id)  // Group by the unique Id property
            .Select(group => new
            {
                CoffeeAddIn = group.First(),  // Use the first instance from the group
                Count = group.Count()
            });

        var topCoffeeAddIns = coffeeAddIns
            .OrderByDescending(coffeeAddIn => coffeeAddIn.Count)
            .Take(LIMIT)
            .Select(coffeeAddIn => coffeeAddIn.CoffeeAddIn);

        return topCoffeeAddIns;
    }

}
