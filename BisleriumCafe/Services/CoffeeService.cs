namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
using BisleriumCafe.Enums;
internal class CoffeeService(Warehouse<CoffeeType> coffeeWarehouse,Warehouse<CoffeeAddIn> addInWarehouse, Warehouse<User> userWarehouse, AuthService _authService)
{
    private readonly Warehouse<CoffeeType> _coffeeWarehouse = coffeeWarehouse;
    private readonly Warehouse<CoffeeAddIn> _addInWarehouse = addInWarehouse;
    public async Task<bool> StoreACoffeeType(CoffeeType coffeeType)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(_coffeeWarehouse.Contains(x => x.CoffeeName, coffeeType.CoffeeName))
        {
            return false;
        }

        coffeeType.CreatedBy = currentUser.Id;
        _coffeeWarehouse.Add(coffeeType);
        await _coffeeWarehouse.FlushAsync();
        return true;
    }

    public string GetCreatedBy(Guid? guid)
    {
        if(guid is null)
        {
            return "-";
        }
        User? user = userWarehouse.Get(user => user.Id, guid);
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

        if(!_coffeeWarehouse.Contains(x => x.Id, coffeeType.Id))
        {
            return false;
        }

        _coffeeWarehouse.Update(coffeeType);
        await _coffeeWarehouse.FlushAsync();
        return true;
    }

    public async Task<bool> DeleteACoffeeType(CoffeeType coffeeType)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(!_coffeeWarehouse.Contains(x => x.Id, coffeeType.Id))
        {
            return false;
        }

        _coffeeWarehouse.Remove(coffeeType);
        await _coffeeWarehouse.FlushAsync();
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

        if(_addInWarehouse.Contains(x => x.AddInName, coffeeAddIn.AddInName))
        {
            return false;
        }

        coffeeAddIn.CreatedBy = currentUser.Id;
        _addInWarehouse.Add(coffeeAddIn);
        await _addInWarehouse.FlushAsync();
        return true;
    }

    public async Task<bool> UpdateACoffeeAddIn(CoffeeAddIn coffeeAddIn)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(!_addInWarehouse.Contains(x => x.Id, coffeeAddIn.Id))
        {
            return false;
        }

        _addInWarehouse.Update(coffeeAddIn);
        await _addInWarehouse.FlushAsync();
        return true;
    }

    public async Task<bool> DeleteACoffeeAddIn(CoffeeAddIn coffeeAddIn)
    {
        User? currentUser = _authService.CurrentUser;
        if(currentUser is null || (currentUser.Role is not UserRole.Admin))
        {
            return false;
        }

        if(!_addInWarehouse.Contains(x => x.Id, coffeeAddIn.Id))
        {
            return false;
        }

        _addInWarehouse.Remove(coffeeAddIn);
        await _addInWarehouse.FlushAsync();
        return true;
    }


}