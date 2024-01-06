namespace BisleriumCafe.Services;
using BisleriumCafe.Model;
using BisleriumCafe.Helpers;
using BisleriumCafe.Enums;
internal class AuthService(Repository<User> userRepository, Repository<Customer> customerRepository, Repository<Order> orderRepository, SessionService sessionService)
{
    private readonly Repository<User> _userRepository = userRepository;
    private readonly Repository<Customer> _customerRepository = customerRepository;
    private readonly Repository<Order> _orderRepository = orderRepository;
    private readonly SessionService _sessionService = sessionService;

    public User? CurrentUser { get; private set; }
    public Customer? CurrentCustomer { get; private set; }

    public async Task<TaskResponse> Register(string username, string fullname,string password, UserRole role)
    {
        TaskResponse response = new();
        response.IsSuccess = false;
        if (_userRepository.HasUserName(username))
        {
          response.Message = "Username already exists!";
            return response;
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
        if(role == UserRole.Customer)
        {
            Customer customer = new()
            {
                Id=user.Id,
                UserName = username,
                FullName = fullname,
                PasswordHash = Hasher.HashSecret(password),
                Role = role,
            };
            _customerRepository.Add(customer);
            await _customerRepository.FlushAsync();
        }   
        response.IsSuccess = true;
        response.Message = "User registered successfully!";
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

        User? user = _userRepository.Get(x => x.Id, userId);
        if (user is null)
        {
            response.Message = "User not found!";
            return response;
        }

        _userRepository.Remove(user);
        await _userRepository.FlushAsync();
        if(user.Role == UserRole.Customer)
        {
            Customer? customer = _customerRepository.Get(x => x.Id, userId);
            if (customer is not null)
            {
                _customerRepository.Remove(customer);
                await _customerRepository.FlushAsync();
            }
        }
        response.IsSuccess = true;
        response.Message = "User removed successfully!";
        return response;
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

            if (CurrentUser.Role == UserRole.Customer)
            {
                Customer? customer = _customerRepository.Get(x => x.Id, CurrentUser.Id);
                if (customer is not null)
                {
                    bool IsRegularMember = IsRegularCustomer(customer);
                    customer.IsRegularMember = IsRegularMember;
                    CurrentCustomer = customer;
                }
            }
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
    }

    public void LogOut()
    {
        _sessionService.DeleteSession();
        CurrentUser = null;
        CurrentCustomer = null;
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

        if (user.Role == UserRole.Customer)
        {
            Customer? customer = _customerRepository.Get(x => x.Id, user.Id);
            if(customer is not null)
            {
                bool IsRegularMember = IsRegularCustomer(customer);
                customer.IsRegularMember = IsRegularMember;
                CurrentCustomer = customer;
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

        if (_userRepository.HasUserName(userName) && CurrentUser.UserName != userName)
        {
            response.Message = "Username already exists!";
            return response;
        }
        CurrentUser.FullName = fullName;
        CurrentUser.UserName = userName;
        await _userRepository.FlushAsync();
        if(CurrentUser.Role == UserRole.Customer && CurrentCustomer is not null)
        {
            CurrentCustomer.FullName = fullName;
            CurrentCustomer.UserName = userName;
            await _customerRepository.FlushAsync();
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

        User? user = _userRepository.Get(x => x.Id, userId);
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
        _userRepository.Update(user);  
        await _userRepository.FlushAsync();
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
        await _userRepository.FlushAsync();
        if(CurrentUser.Role == UserRole.Customer && CurrentCustomer is not null)
        {
            CurrentCustomer.PasswordHash = Hasher.HashSecret(newPassword);
            await _customerRepository.FlushAsync();
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

    public bool IsRegularCustomer(Customer customer)
    {
        // Get all orders within the last 30 days for the given customer
        var allOrdersDatePast30Days = _orderRepository.GetAll()
            .Where(order => order.CustomerId == customer.Id && order.OrderDate >= DateTime.UtcNow.AddDays(-30))
            .Select(order => order.OrderDate)
            .ToList();

        // Materialize the list of weekdays
        var weekdaysList = Date.GetWeekdaysList(DateTime.UtcNow, 30);

        // Use HashSet for faster lookup
        var orderDatesHashSet = new HashSet<DateTime>(allOrdersDatePast30Days);

        // Check if all weekdays have at least one order
        return weekdaysList.All(date => orderDatesHashSet.Contains(date));
    }

}