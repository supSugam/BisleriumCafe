namespace BisleriumCafe.Model;


public class User : IModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string? PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid? CreatedBy { get; set; }

    public User()
    {
    }
    public User(string userName, string fullName, string passwordHash, UserRole role)
    {
        UserName = userName;
        FullName = fullName;
        PasswordHash = passwordHash;
        Role = role;
    }
}