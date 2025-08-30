using EGeek.Id.Domain.Entities;
using EGeek.Id.DTOs.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EGeek.Id.UseCases;

internal static class GetMeUseCase
{
    [Authorize]
    internal static async Task<Ok<GetMeResponse>> Action(ClaimsPrincipal principal, 
                                                    UserManager<User> userManager)
    {
        var userId = principal.FindFirstValue("id");
        if (userId == null)
            throw new ApplicationException("User ID claim not found.");

        User? user = await userManager.FindByIdAsync(userId);
        if (user == null)
            throw new ApplicationException("User not found.");

        var claims = await userManager.GetClaimsAsync(user);
        var role = claims.FirstOrDefault(c => c.Type == "role")?.Value;

        return TypedResults.Ok(new GetMeResponse(user.Email!, user.PhoneNumber, role));
    }
}
