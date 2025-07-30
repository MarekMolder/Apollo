using App.DTO.v1.Enums;

namespace App.DTO.v1;

/// <summary>
/// Represents the type of action (e.g. Add, Remove) that can be performed on a product.
/// </summary>
public class ActionTypeEntityCreate
{
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
    public ActionTypeEnum Code { get; set; }
}