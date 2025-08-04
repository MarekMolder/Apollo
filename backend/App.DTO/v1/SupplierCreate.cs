
namespace App.DTO.v1;

/// <summary>
/// Represents a supplier who provides products to the storageroom system.
/// Includes contact information and address association.
/// </summary>
public class SupplierCreate
{
    /// <summary>
    /// Supplier's name (e.g., company or individual name).
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Telephone number for contacting the supplier.
    /// </summary>
    public string TelephoneNr { get; set; } = default!;
    
    /// <summary>
    /// Email address of the supplier.
    /// </summary>
    public string Email { get; set; } = default!;
    
    /// <summary>
    /// Foreign key to the supplier's address.
    /// </summary>
    public Guid AddressId { get; set; }
}