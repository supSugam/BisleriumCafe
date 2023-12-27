namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
using BisleriumCafe.Enums;
internal class CoffeeService(Repository<CoffeeType> coffeeRepository,Repository<CoffeeAddIn> addInRepository, Repository<User> userRepository, AuthService _authService)
{
    private readonly Repository<CoffeeType> _coffeeRepository = coffeeRepository;
    private readonly Repository<CoffeeAddIn> _addInRepository = addInRepository;


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

    public async Task<bool> UpdateACoffeeType(CoffeeType coffeeType)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(!_coffeeRepository.Contains(x => x.Id, coffeeType.Id))
        {
            return false;
        }

        _coffeeRepository.Update(coffeeType);
        await _coffeeRepository.FlushAsync();
        return true;
    }

    public async Task<bool> DeleteACoffeeType(CoffeeType coffeeType)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(!_coffeeRepository.Contains(x => x.Id, coffeeType.Id))
        {
            return false;
        }

        _coffeeRepository.Remove(coffeeType);
        await _coffeeRepository.FlushAsync();
        return true;
    }

    // Coffee Addins
    public async Task<bool> StoreACoffeeAddIn(CoffeeAddIn coffeeAddIn)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(_addInRepository.Contains(x => x.AddInName, coffeeAddIn.AddInName))
        {
            return false;
        }

        coffeeAddIn.CreatedBy = currentUser.Id;
        _addInRepository.Add(coffeeAddIn);
        await _addInRepository.FlushAsync();
        return true;
    }

    public async Task<bool> UpdateACoffeeAddIn(CoffeeAddIn coffeeAddIn)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(!_addInRepository.Contains(x => x.Id, coffeeAddIn.Id))
        {
            return false;
        }

        _addInRepository.Update(coffeeAddIn);
        await _addInRepository.FlushAsync();
        return true;
    }

    public async Task<bool> DeleteACoffeeAddIn(CoffeeAddIn coffeeAddIn)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(!_addInRepository.Contains(x => x.Id, coffeeAddIn.Id))
        {
            return false;
        }

        _addInRepository.Remove(coffeeAddIn);
        await _addInRepository.FlushAsync();
        return true;
    }


}