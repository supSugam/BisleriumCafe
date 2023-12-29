namespace BisleriumCafe.Services;

internal static class OrdereServiceInjection
{
    public static IServiceCollection AddOrderService(this IServiceCollection services)
    {
        return services.AddSingleton<OrderService>();
    }
}