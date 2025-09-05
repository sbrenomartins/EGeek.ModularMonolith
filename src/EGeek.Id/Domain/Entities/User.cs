using EGeek.Id.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace EGeek.Id.Domain.Entities;

public class User : IdentityUser
{
    [NotMapped]
    internal string PasswordToSave { get; private set; }

    [NotMapped]
    internal List<Claim> RoleClaims { get; set; }

    internal User() {}

    internal User(PostUserRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new ArgumentException("Email is required.");
        if (string.IsNullOrEmpty(request.Role))
            throw new ArgumentException("Role is required.");
        if (string.IsNullOrEmpty(request.Password))
            throw new ArgumentException("Password is required.");
        if (request.Password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters long.");

        var hasUpperCase = request.Password.Any(char.IsUpper);
        var hasLowerCase = request.Password.Any(char.IsLower);
        var hasDigit = request.Password.Any(char.IsDigit);
        var hasSpecialChar = new Regex(@"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\?]").IsMatch(request.Password);

        if (!hasUpperCase || !hasLowerCase || !hasDigit || !hasSpecialChar)
            throw new ArgumentException(@"Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

        Id = Guid.CreateVersion7().ToString();
        UserName = request.Email;
        Email = request.Email;
        PasswordToSave = request.Password;
        RoleClaims = new List<Claim>
        {
            new Claim("role", request.Role)
        };

        if (!string.IsNullOrEmpty(request.TenantId))
        {
            RoleClaims.Add(new Claim("tenantId", request.TenantId));
        }
    }
}
