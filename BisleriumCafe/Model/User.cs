﻿using System.Text.Json;
using BisleriumCafe.Enums;
namespace BisleriumCafe.Model;


public class User : IModel, ICloneable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; }
    //public string Email { get; set; }
    public string FullName { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    //public bool HasInitialPassword { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    //public Guid CreatedBy { get; set; }

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

    public object Clone()
    {
        return new User
        {
            Id = Id,
            UserName = UserName,
            //Email = Email,
            FullName = FullName,
            PasswordHash = PasswordHash,
            Role = Role,
            CreatedAt = CreatedAt
        };
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}