using Base.Contracts;

namespace App.BLL.DTO;

/// <summary>
/// Represents the type of action (e.g. Add, Remove) that can be performed on a product.
/// </summary>
public class ActionTypeEntity : IDomainId 
{
    /// <summary>
    /// Unique identifier of the action type.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Localized display name of the action type.
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Optional end date, indicating when this action type was disabled or no longer in use.
    /// </summary>
    public DateTime? EndedAt { get; set; }
    
    /// <summary>
    /// Coded enum value representing the type (e.g. Add = 2, Remove = 1).
    /// </summary>
    public Enums.ActionTypeEnum Code { get; set; }
    
    /// <summary>
    /// Collection of ActionEntities associated with this type.
    /// </summary>
    public ICollection<BLL.DTO.ActionEntity>? Actions { get; set; }
}