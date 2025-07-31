namespace Base.Domain.Identity;

/// <summary>
/// Non-generic base class for refresh tokens using <see cref="Guid"/> as the primary key.
/// </summary>
public class BaseRefreshToken: BaseRefreshToken<Guid>
{
    
}

/// <summary>
/// Generic base class for refresh token entity that supports tracking of current and previous tokens.
/// </summary>
public class BaseRefreshToken<TKey> : BaseEntity<TKey>
where TKey : IEquatable<TKey>
{
    /// <summary>
    /// The active refresh token value.
    /// </summary>
    public string RefreshToken { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>
    /// The expiration time of the current refresh token (default is 7 days from creation).
    /// </summary>
    public DateTime Expiration { get; set; } = DateTime.UtcNow.AddDays(7);
    
    /// <summary>
    /// The previous refresh token value, if rotated.
    /// </summary>
    public string? PreviousRefreshToken { get; set; }
    
    /// <summary>
    /// The expiration time of the previous refresh token (default is 7 days from creation).
    /// </summary>
    public DateTime PreviousExpiration { get; set; } = DateTime.UtcNow.AddDays(7);
}