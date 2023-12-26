namespace BisleriumCafe.Services;

internal static class CoffeeServiceInjection
{
    public static IServiceCollection AddCoffeeService(this IServiceCollection services)
    {
        return services.AddSingleton<CoffeeService>();
    }
}