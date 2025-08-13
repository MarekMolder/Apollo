using Base.Contracts;

namespace App.DAL.DTO;

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
    /// Reference to the address entity.
    /// </summary>
    public Guid AddressId { get; set; }
    public DAL.DTO.Address? Address { get; set; }
    
    /// <summary>
    /// List of role names allowed to access this storage room.
    /// </summary>
    public List<string>? AllowedRoles { get; set; }
    
    /// <summary>
    /// Collection of actions (add/remove) that occurred in this storage room.
    /// </summary>
    public ICollection<DAL.DTO.ActionEntity>? Actions { get; set; }
}