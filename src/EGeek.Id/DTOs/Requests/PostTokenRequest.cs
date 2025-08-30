namespace EGeek.Id.DTOs.Requests;

internal record PostTokenRequest(
    string Email,
    string Password
);
