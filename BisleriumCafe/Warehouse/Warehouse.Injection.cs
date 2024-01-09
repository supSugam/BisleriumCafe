namespace BisleriumCafe;
using BisleriumCafe.Model;
internal static class WarehouseInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        return services.AddSingleton<Warehouse<User>, Warehouse<User>>().AddSingleton<Warehouse<CoffeeType>, Warehouse<CoffeeType>>().
            AddSingleton<Warehouse<Member>, Warehouse<Member>>()
            .AddSingleton<Warehouse<CoffeeAddIn>,Warehouse<CoffeeAddIn>>()
            .AddSingleton<Warehouse<Order>, Warehouse<Order>>();
    }
}
