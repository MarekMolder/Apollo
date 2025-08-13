using Base.Contracts;

namespace App.BLL.DTO;

/// <summary>
/// Represents a reason for an inventory action (e.g., damaged goods, expired items, returned by customer).
/// </summary>
public class Reason : IDomainId
{
    /// <summary>
    /// Unique identifier of the reason.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Description of the reason (e.g., "Expired", "Damaged", "Incorrect delivery").
    /// </summary>
    public string Description { get; set; } = default!;
    
    /// <summary>
    /// Collection of actions (add/remove) that reference this reason.
    /// </summary>
    public ICollection<BLL.DTO.ActionEntity>? Actions { get; set; }
}