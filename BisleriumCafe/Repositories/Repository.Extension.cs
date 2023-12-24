namespace BisleriumCafe;
using BisleriumCafe.Model;
internal static class Repository
{
    public static bool HasUserName(this Repository<User> userRepository, string userName)
    {
        return userRepository.Contains(x => x.UserName, userName);
    }

    public static void OnDebugConsoleWriteUserNames(this Repository<User> userRepository)
    {
        System.Diagnostics.Debug.WriteLine("Users");
        foreach (User i in userRepository.GetAll())
        {
            System.Diagnostics.Debug.WriteLine($"{{ Username = {i.UserName} , Role = {i.Role}}}");
        }
    }
}
