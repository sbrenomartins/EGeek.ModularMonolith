using EGeek.Id.UseCases;
using Microsoft.AspNetCore.Builder;

namespace EGeek.Id.Configuration;

public static class IdConfigApp
{
    public static void Apply(WebApplication app)
    {
        app.MapPost("/v1/users", PostUserUseCase.Action);
        app.MapPost("/v1/token", PostTokenUseCase.Action);
        app.MapGet("/v1/users/me", GetMeUseCase.Action);
    }
}
