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
    /// Coded enum value representing the type (e.g. Add = 2, Remove = 1).
    /// </summary>
    public ActionTypeEnum Code { get; set; }
}