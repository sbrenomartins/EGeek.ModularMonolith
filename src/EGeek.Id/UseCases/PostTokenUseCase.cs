using EGeek.Id.Domain.Entities;
using EGeek.Id.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EGeek.Id.UseCases;

internal static class PostTokenUseCase
{
    internal static async Task<Results<Ok<string>, UnauthorizedHttpResult>> Action(PostTokenRequest request,
                                            UserManager<User> userManager,
                                            IConfiguration configuration)
    {
        if (string.IsNullOrEmpty(request.Email))
            throw new ArgumentException("Email is required.");
        if (string.IsNullOrEmpty(request.Password))
            throw new ArgumentException("Password is required.");

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return TypedResults.Unauthorized();

        var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return TypedResults.Unauthorized();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var userClaims = await userManager.GetClaimsAsync(user);
        userClaims.Add(new Claim("id", user.Id!));
        userClaims.Add(new Claim(ClaimTypes.Email, user.Email!));

        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                                         configuration["Jwt:Audience"],
                                         userClaims,
                                         expires: DateTime.Now.AddMinutes(30),
                                         signingCredentials: creds);

        return TypedResults.Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}
