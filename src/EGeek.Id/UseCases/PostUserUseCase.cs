using EGeek.Id.Domain.Entities;
using EGeek.Id.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace EGeek.Id.UseCases;

internal static class PostUserUseCase
{
    internal static async Task<Created<string>> Action(PostUserRequest request, UserManager<User> userManager)
    {
        User user = new(request);

        var result = await userManager.CreateAsync(user, user.PasswordToSave);
        if (!result.Succeeded)
            throw new ApplicationException($"Failed to create user. \n${string.Join("\n", result.Errors.Select(e => e.Description))}");

        result = await userManager.AddClaimsAsync(user, user.RoleClaims);
        if (!result.Succeeded)
            throw new ApplicationException("Failed to add claims to user.");

        return TypedResults.Created($"/v1/users/{user.Id}", user.Id);
    }
}
