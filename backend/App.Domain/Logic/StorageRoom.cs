using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain.Logic;

/// <summary>
/// Represents a physical storage room location, optionally restricted by allowed user roles.
/// </summary>
public class StorageRoom : BaseEntity
{
    /// <summary>
    /// Human-readable name of the storage room (e.g., "Mustamäe kino").
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Foreign key to the address where the storage room is located.
    /// Reference to the address entity.
    /// </summary>
    public Guid AddressId { get; set; }
    public Domain.Logic.Address? Address { get; set; }
    
    /// <summary>
    /// List of role names allowed to access this storage room.
    /// </summary>
    public List<string> AllowedRoles { get; set; } = new();
    
    /// <summary>
    /// Collection of actions (add/remove) that occurred in this storage room.
    /// </summary>
    public ICollection<Domain.Logic.ActionEntity>? Actions { get; set; }
}