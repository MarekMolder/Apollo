using Base.Contracts;

namespace App.BLL.DTO;

/// <summary>
/// Represents a physical address that may be associated with storage rooms or suppliers.
/// </summary>
public class Address : IDomainId
{
    /// <summary>
    /// Unique identifier for the address.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Street name.
    /// </summary>
    public string StreetName { get; set; } = default!;

    /// <summary>
    /// Building number on the street.
    /// </summary>
    public int BuildingNr { get; set; }
    
    /// <summary>
    /// Postal or ZIP code.
    /// </summary>
    public string PostalCode { get; set; } = default!;
    
    /// <summary>
    /// City name.
    /// </summary>
    public string City { get; set; } = default!;
    
    /// <summary>
    /// Province or state (if applicable).
    /// </summary>
    public string Province { get; set; } = default!;
    
    /// <summary>
    /// Country name.
    /// </summary>
    public string Country { get; set; } = default!;
    
    /// <summary>
    /// Display name for this address (e.g., "Main Warehouse").
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Unit or apartment number, if applicable.
    /// </summary>
    public int? UnitNr { get; set; }
    
    /// <summary>
    /// Storage rooms associated with this address.
    /// </summary>
    public ICollection<BLL.DTO.StorageRoom>? StorageRooms { get; set; }
    
    /// <summary>
    /// Suppliers associated with this address.
    /// </summary>
    public ICollection<BLL.DTO.Supplier>? Suppliers { get; set; }
}