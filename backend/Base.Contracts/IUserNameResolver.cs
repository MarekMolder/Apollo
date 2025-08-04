namespace Base.Contracts;

/// <summary>
/// Service abstraction for resolving the current user's name from the context.
/// </summary>
public interface IUserNameResolver
{
    /// <summary>
    /// The username of the current authenticated user, or "system" if not available.
    /// </summary>
    string CurrentUserName { get; }
}