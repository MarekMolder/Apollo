using Base.Domain;

namespace App.Domain.Logic;

/// <summary>
/// Represents a supplier who provides products to the storageroom system.
/// Includes contact information and address association.
/// </summary>
public class Supplier : BaseEntity
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
    /// Reference to the related address entity.
    /// </summary>
    public Guid AddressId { get; set; }
    public Address? Address { get; set; }
    
    /// <summary>
    /// Collection of actions associated with this supplier (e.g., stock additions).
    /// </summary>
    public ICollection<Domain.Logic.ActionEntity>? Actions { get; set; }
    
    /// <summary>
    /// Collection of products provided by this supplier.
    /// </summary>
    public ICollection<Domain.Logic.Product>? Products { get; set; }
}