using Base.Domain;

namespace App.Domain.Logic;

/// <summary>
/// Represents a reason for an inventory action (e.g., damaged goods, expired items, returned by customer).
/// </summary>
public class Reason : BaseEntity
{
    /// <summary>
    /// Description of the reason (e.g., "Expired", "Damaged", "Incorrect delivery").
    /// </summary>
    public string Description { get; set; } = default!;
    
    /// <summary>
    /// Optional date indicating when this reason was retired or deactivated.
    /// </summary>
    public DateTime? EndedAt { get; set; }
    
    /// <summary>
    /// Collection of actions (add/remove) that reference this reason.
    /// </summary>
    public ICollection<Domain.Logic.ActionEntity>? Actions { get; set; }
}