namespace BisleriumCafe.Providers;
using BisleriumCafe.Model;
internal static class JsonFileProvider
{
    public static IServiceCollection AddJsonFileProvider(this IServiceCollection services)
    {
        return services.AddSingleton<FileProvider<User>, JsonFileProvider<User>>()
            .AddSingleton<FileProvider<CoffeeType>, JsonFileProvider<CoffeeType>>().AddSingleton<FileProvider<Customer>, JsonFileProvider<Customer>>()
        .AddSingleton<FileProvider<CoffeeAddIn>, JsonFileProvider<CoffeeAddIn>>();
    }
}
