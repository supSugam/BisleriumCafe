namespace BisleriumCafe;
using BisleriumCafe.Model;
internal static class RepositoryInjection
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        return services.AddSingleton<Repository<User>, Repository<User>>().AddSingleton<Repository<CoffeeType>, Repository<CoffeeType>>().
            AddSingleton<Repository<Customer>, Repository<Customer>>();
            //.AddSingleton<Repository<ActivityLog>, Repository<ActivityLog>>();
    }
}
