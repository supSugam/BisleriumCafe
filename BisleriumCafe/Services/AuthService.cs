namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
using BisleriumCafe.Enums;
internal class AuthService(Warehouse<User> userWarehouse, Warehouse<Member> memberWarehouse, Warehouse<Order> orderWarehouse, SessionService sessionService)
{
    private readonly Warehouse<User> _userWarehouse = userWarehouse;
    private readonly Warehouse<Member> _memberWarehouse = memberWarehouse;
    private readonly Warehouse<Order> _orderWarehouse = orderWarehouse;
    private readonly SessionService _sessionService = sessionService;

    public User? CurrentUser { get; private set; }
    public Member? CurrentMember { get; private set; }

    // Member


    public async Task InitializeAdminOnFirstRun()
    {
        if (_userWarehouse.GetAll().Count == 0 || _userWarehouse.GetAll().Where(user => user.Role == UserRole.Admin).Count() == 0)
        {
            User user = new()
            {
                UserName = "admin",
                FullName = "Administrator",
                Role = UserRole.Admin,
                PasswordHash = Hasher.HashSecret("admin"),
            };
            _userWarehouse.Add(user);
            await _userWarehouse.FlushAsync();
        }
    }
    public async Task<TaskResponse> UpdateActiveMember(string userName)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if (CurrentUser is null)
        {
            response.Message = "You are not authorized to do this.";
            return response;
        }
        Member? member = _memberWarehouse.Get(x => x.UserName, userName);
        if (member is null)
        {
            response.Message = "Member not found, Register first!";
            return response;
        }
        CurrentMember = member;
        response.IsSuccess = true;
        response.Message = $"Active member updated to {member.FullName}!";
        return response;
    }

    public async Task<TaskResponse> RemoveCurrentMember()
    {
        CurrentMember = null;
        TaskResponse response = new();
        response.IsSuccess = true;
        response.Message = "Switched back to customer mode!";
        return response;
    }

    public async Task<TaskResponse> Register(UserRole role, string username, string fullname,string? password=null)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if (_userWarehouse.HasUserName(username))
        {
          response.Message = "Username already exists!";
            return response;
        }

        User user = new()
        {
            UserName = username,
            FullName = fullname,
            Role = role,
        };

        if(role != UserRole.Member && password is not null)
        {
            user.PasswordHash = Hasher.HashSecret(password);
        }

        if(CurrentUser is not null)
        {
            user.CreatedBy = CurrentUser.Id;
        }
        _userWarehouse.Add(user);
        await _userWarehouse.FlushAsync();
        response.Message = "User registered successfully!";


        if (role == UserRole.Member)
        {
            Member member = new()
            {
                Id=user.Id,
                UserName = username,
                FullName = fullname,
                Role = role,
            };
            response.Message = "Member registered successfully!";
            _memberWarehouse.Add(member);
            await _memberWarehouse.FlushAsync();
        }   
        response.IsSuccess = true;
        return response;
    }

    public async Task<TaskResponse> RemoveUser(Guid userId)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if (CurrentUser is null)
        {
            response.Message = "User not found!";
            return response;
        }

        if(CurrentUser.Role != UserRole.Admin)
        {
            response.Message = "You are not authorized to perform this action!";
            return response;
        }

        if (CurrentUser.Id == userId)
        {
            response.Message = "You cannot delete yourself!";
            return response;
        }

        User? user = _userWarehouse.Get(x => x.Id, userId);
        if (user is null)
        {
            response.Message = "User not found!";
            return response;
        }

        _userWarehouse.Remove(user);
        await _userWarehouse.FlushAsync();
        if (user.Role == UserRole.Member)
        {
            Member? member = _memberWarehouse.Get(x => x.Id, userId);
            if (member is not null)
            {
                _memberWarehouse.Remove(member);
                await _memberWarehouse.FlushAsync();
            }
        }
        response.IsSuccess = true;
        response.Message = "User removed successfully!";
        return response;
    }

    public async Task<TaskResponse> Login(string userName, string password, bool stayLoggedIn)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if (CurrentUser is not null)
        {
            response.Message = "Already logged in!";
            return response;
        }
        CurrentUser = _userWarehouse.Get(x => x.UserName, userName);


        if (CurrentUser is null)
        {
          response.Message = "User not found!";
            return response;
        }

        if (Hasher.VerifyHash(password, CurrentUser.PasswordHash))
        {
            Session session = Session.Generate(CurrentUser.Id, stayLoggedIn,CurrentUser.Role);
            await _sessionService.SaveSession(session);

            if (CurrentUser.Role == UserRole.Member)
            {
                response.IsSuccess = false;
                response.Message = "You are not authorized to login!";
                return response;
            } 
            response.IsSuccess = true;
            response.Message = "Login successful!";
        }
        else
        {
            response.Message = "Incorrect password!";
            return response;
        }
        return response;
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
    }

    public void LogOut()
    {
        _sessionService.DeleteSession();
        CurrentUser = null;
        CurrentMember = null;
    }

    public async Task CheckSession()
    {
        Session? session = await _sessionService.LoadSession();
        if (session is null)
        {
            return;
        }

        User? user = _userWarehouse.Get(x => x.Id, session.UserId);
        
        if (user is null)
        {
            return;
        }

        if (user.Role == UserRole.Member)
        {
            Member? member = _memberWarehouse.Get(x => x.Id, user.Id);
            if(member is not null)
            {
                member.IsRegularMember = IsRegularMember(member);
                CurrentMember = member;
            }
        }

        if (!session.IsValid())
        {
            throw new Exception("Session expired!");
        }

        CurrentUser = user;
    }

    public async Task<TaskResponse> UpdatePersonalDetails(string fullName, string userName)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if (CurrentUser is null)
        {
            response.Message = "User not found!";
            return response;
        }

        if (CurrentUser.FullName == fullName && CurrentUser.UserName == userName)
        {
            response.Message = "No changes made!";
            return response;

        }

        if (_userWarehouse.HasUserName(userName) && CurrentUser.UserName != userName)
        {
            response.Message = "Username already exists!";
            return response;
        }
        CurrentUser.FullName = fullName;
        CurrentUser.UserName = userName;
        await _userWarehouse.FlushAsync();
        if(CurrentUser.Role == UserRole.Member && CurrentMember is not null)
        {
            CurrentMember.FullName = fullName;
            CurrentMember.UserName = userName;
            await _memberWarehouse.FlushAsync();
        }
        response.IsSuccess = true;
        response.Message = "Personal details updated successfully!";
        return response;
    }

    public async Task<TaskResponse> UpdateUserDetails(Guid userId, string fullName, string userName, string passwordHash)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if (CurrentUser is null)
        {
            response.Message = "Need to login.";
            return response;
        }

        if (CurrentUser.Role != UserRole.Admin || CurrentUser.Id == userId)
        {
            response.Message = "You are not authorized to perform this action!";
            return response;
        }

        User? user = _userWarehouse.Get(x => x.Id, userId);
        if (user is null)
        {
            response.Message = "User not found!";
            return response;
        }

        if (user.FullName == fullName && user.UserName == userName && string.IsNullOrEmpty(passwordHash))
        {
            response.Message = "No changes made!";
            return response;
        }
        user.UserName = userName;
        user.FullName = fullName;
        if (!string.IsNullOrEmpty(passwordHash))
        {
            user.PasswordHash = Hasher.HashSecret(passwordHash);
        }
        _userWarehouse.Update(user);  
        await _userWarehouse.FlushAsync();
        response.IsSuccess = true;
        response.Message = "User details updated successfully!";
        return response;
    }

    public async Task<TaskResponse> UpdateUserPassword(string oldPassword, string newPassword)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if (CurrentUser is null)
        {
            response.Message = "User not found!";
            return response;
        }

        if (!Hasher.VerifyHash(oldPassword, CurrentUser.PasswordHash))
        {
            response.Message = "Old password is incorrect!";
            return response;
        }

        if (oldPassword == newPassword)
        {
            response.Message = "New password must be different from current password.";
            return response;
        }
        response.IsSuccess = true;
        response.Message = "Password changed successfully!";

        CurrentUser.PasswordHash = Hasher.HashSecret(newPassword);
        await _userWarehouse.FlushAsync();
        if(CurrentUser.Role == UserRole.Member && CurrentMember is not null)
        {
            CurrentMember.PasswordHash = Hasher.HashSecret(newPassword);
            await _memberWarehouse.FlushAsync();
        }
        return response;
    }
    //public bool IsRegularCustomer(Customer customer)
    //{
    //    IEnumerable<DateTime> AllOrdersDatePast30Days = GetAllOrders().Where(order => order.CustomerId == customer.Id && order.OrderDate >= DateTime.Now.AddDays(-30)).Select(order=>order.OrderDate);

    //    List<DateTime> Dates = Date.GetWeekdaysList(DateTime.Now, 30);
    //    if(AllOrdersDatePast30Days.Count()< Dates.Count)
    //    {
    //        return false;
    //    }
    //    bool MeetsCondition = Dates.All(date1 => AllOrdersDatePast30Days.Where(date2 => Date.AreSameDay(date1, date2)).Count()>0);
    //    return MeetsCondition;
    //}

    public bool IsRegularMember(Member member)
    {
        // Get all orders within the last 30 days for the given customer
        var allOrdersDatePast30Days = _orderWarehouse.GetAll()
            .Where(order => order.CustomerId == member.Id && order.OrderDate >= DateTime.UtcNow.AddDays(-30))
            .Select(order => order.OrderDate)
            .ToList();

        // Materialize the list of weekdays
        var weekdaysList = Date.GetWeekdaysList(DateTime.UtcNow, 30);

        // HashSet for faster lookup
        var orderDatesHashSet = new HashSet<DateTime>(allOrdersDatePast30Days);

        // Check if all weekdays have at least one order
        return weekdaysList.All(date => orderDatesHashSet.Contains(date));
    }

}