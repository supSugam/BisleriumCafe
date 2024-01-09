namespace BisleriumCafe;
using BisleriumCafe.Model;
internal static class Warehouse
{
    public static bool HasUserName(this Warehouse<User> userWarehouse, string userName)
    {
        return userWarehouse.Contains(x => x.UserName, userName);
    }
}
