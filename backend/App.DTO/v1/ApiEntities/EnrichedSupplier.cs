using Base.Contracts;

namespace App.DTO.v1.ApiEntities;

public class EnrichedSupplier : IDomainId
{
    /// <summary>
    /// Unique identifier for the supplier.
    /// </summary>
    public Guid Id { get; set; }
    
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
    /// Full formatted address string (e.g., "Ehitajate tee 5, 12616 Tallinn, Estonia").
    /// </summary>
    public Guid AddressId { get; set; }
    public string FullAddress { get; set; } = default!;
}