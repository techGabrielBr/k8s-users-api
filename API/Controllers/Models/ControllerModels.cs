namespace UsersAPI.API.Controllers.Models
{
    #region AUTH
    public record RegisterUserRequest(
        string Name,
        string Email,
        string Password
    );

    public record LoginUserRequest(
        string Email,
        string Password
    );

    public record AuthResponse(
        string Token,
        DateTime ExpiresAt
    );

    #endregion
}