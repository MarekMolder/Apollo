namespace App.DTO.v1;

/// <summary>
/// Represents a physical address that may be associated with storage rooms or suppliers.
/// </summary>
public class AddressCreate
{
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
}