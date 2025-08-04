using Base.Contracts;

namespace WebApp.Helpers;

/// <summary>
/// Resolves the current user's username from the HTTP context.
/// Used for auditing and logging purposes.
/// </summary>
public class UserNameResolver : IUserNameResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserNameResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    /// <summary>
    /// Gets the name of the currently authenticated user.
    /// If no user is authenticated, returns "system".
    /// </summary>
    public string CurrentUserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
}