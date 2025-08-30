namespace EGeek.Id.DTOs.Requests;

internal record PostUserRequest(
    string Email,
    string Password,
    string Role,
    string? TenantId = null
);
