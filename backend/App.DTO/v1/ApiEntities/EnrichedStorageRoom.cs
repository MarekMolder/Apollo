using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

/// <summary>
/// Data Transfer Object (DTO) representing an enriched view of a storage room.
/// Includes full address and allowed roles for UI and access control.
/// </summary>
public class EnrichedStorageRoom : IDomainId
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
    /// Optional end date marking the storage room as deactivated or archived.
    /// </summary>
    public DateTime? EndedAt { get; set; }
    
    /// <summary>
    /// Foreign key to the address where the storage room is located.
    /// Full formatted address string (e.g., "Ehitajate tee 5, 12616 Tallinn, Estonia").
    /// </summary>
    public Guid AddressId { get; set; }
    public string FullAddress { get; set; } = default!;
    
    /// <summary>
    /// List of role names allowed to access this storage room.
    /// </summary>
    public List<string>? AllowedRoles { get; set; }
}