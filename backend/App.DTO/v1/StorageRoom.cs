using Base.Contracts;

namespace App.DTO.v1;

/// <summary>
/// Represents a physical storage room location, optionally restricted by allowed user roles.
/// </summary>
public class StorageRoom : IDomainId
{
    /// <summary>
    /// Unique identifier for the storage room.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Human-readable name of the storage room (e.g., "Mustamäe kino").
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Foreign key to the address where the storage room is located.
    /// </summary>
    public Guid AddressId { get; set; }
    
    /// <summary>
    /// List of role names allowed to access this storage room.
    /// </summary>
    public List<string>? AllowedRoles { get; set; }
}