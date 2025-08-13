using App.Domain.Enums;
using Base.Domain;

namespace App.Domain.Logic;

/// <summary>
/// Represents the type of action (e.g. Add, Remove) that can be performed on a product.
/// </summary>
public class ActionTypeEntity : BaseEntity
{
    /// <summary>
    /// Localized display name of the action type.
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Coded enum value representing the type (e.g. Add = 2, Remove = 1).
    /// </summary>
    public ActionTypeEnum Code { get; set; }
    
    /// <summary>
    /// Collection of ActionEntities associated with this type.
    /// </summary>
    public ICollection<Domain.Logic.ActionEntity>? Actions { get; set; }
}