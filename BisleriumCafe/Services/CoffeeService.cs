namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
using BisleriumCafe.Enums;
internal class CoffeeService(Repository<CoffeeType> coffeeRepository,Repository<User> userRepository, AuthService _authService)
{
    private readonly Repository<CoffeeType> _coffeeRepository = coffeeRepository;


    public async Task<bool> StoreACoffeeType(CoffeeType coffeeType)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(_coffeeRepository.Contains(x => x.CoffeeName, coffeeType.CoffeeName))
        {
            return false;
        }

        coffeeType.CreatedBy = currentUser.Id;
        _coffeeRepository.Add(coffeeType);
        await _coffeeRepository.FlushAsync();
        return true;
    }

    public string GetCreatedBy(Guid? guid)
    {
        if(guid is null)
        {
            return "-";
        }
        User? user = userRepository.Get(user => user.Id, guid);
        if (user is null)
        {
            return "-";
        }
        return user.FullName;
    }

}