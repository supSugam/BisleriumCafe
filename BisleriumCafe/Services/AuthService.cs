namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
using BisleriumCafe.Enums;
internal class AuthService(Repository<User> userRepository, SessionService sessionService)
{
    private readonly Repository<User> _userRepository = userRepository;

    private readonly SessionService _sessionService = sessionService;

    public User? CurrentUser { get; private set; }

    public async Task<string?> SeedInitialUser()
    {
        if (_userRepository.GetAll().Count != 0)
        {
            return null;
        }

        if (_userRepository.Contains(x => x.Role, UserRole.Admin))
        {
            return null;
        }

        string username = "admin", pleaseChange = "Please Change!";
        User user = new()
        {
            UserName = username,
            FullName = pleaseChange,
            PasswordHash = Hasher.HashSecret(username),
            Role = UserRole.Admin,
            //CreatedBy = Guid.Empty,
        };
        _userRepository.Add(user);
        await _userRepository.FlushAsync();
        return username;
    }

    public async Task<bool> Register(string username, string fullname,string password, UserRole role)
    {
        if (_userRepository.HasUserName(username))
        {
            return false;
        }

        User user = new()
        {
            UserName = username,
            FullName = fullname,
            PasswordHash = Hasher.HashSecret(password),
            Role = role,
        };
        if(CurrentUser is not null && CurrentUser.Role == UserRole.Admin)
        {
            user.CreatedBy = CurrentUser.Id;
        }
        _userRepository.Add(user);
        await _userRepository.FlushAsync();
        return true;
    }

    public async Task<bool> Login(string userName, string password, bool stayLoggedIn)
    {
        CurrentUser = _userRepository.Get(x => x.UserName, userName);

        if (CurrentUser is null)
        {
            return false;
        }

        if (Hasher.VerifyHash(password, CurrentUser.PasswordHash))
        {
            Session session = Session.Generate(CurrentUser.Id, stayLoggedIn,CurrentUser.Role);
            await _sessionService.SaveSession(session);
            return true;
        }
        return false;
    }

    public bool IsUserAdmin()
    {
        return CurrentUser is not null && CurrentUser.Role == UserRole.Admin;
    }

    public void ChangePassword(string oldPassword, string newPassword)
    {
        if (oldPassword == newPassword)
        {
            throw new Exception("New password must be different from current password.");
        }

        CurrentUser.PasswordHash = Hasher.HashSecret(newPassword);
        //CurrentUser.HasInitialPassword = false;
    }

    public void LogOut()
    {
        _sessionService.DeleteSession();
        CurrentUser = null;
    }

    public async Task CheckSession()
    {
        Session? session = await _sessionService.LoadSession();
        if (session is null)
        {
            return;
        }

        User? user = _userRepository.Get(x => x.Id, session.UserId);
        if (user is null)
        {
            return;
        }

        if (!session.IsValid())
        {
            throw new Exception("Session expired!");
        }

        CurrentUser = user;
    }
}