using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

/// <summary>
/// ViewModel for creating and editing an ActionEntity.
/// Provides select lists for dropdowns in the UI.
/// </summary>
public class ActionEntityCreateEditViewModel
{
    /// <summary>
    /// The action entity being created or edited.
    /// </summary>
    public ActionEntity ActionEntity { get; set; } = default!;

    /// <summary>
    /// Select list for action types.
    /// </summary>
    [ValidateNever]
    public SelectList ActionTypeSelectList { get; set; } = default!;

    /// <summary>
    /// Select list for products.
    /// </summary>
    [ValidateNever]
    public SelectList ProductSelectList { get; set; } = default!;
    
    /// <summary>
    /// Select list for reasons.
    /// </summary>
    [ValidateNever]
    public SelectList ReasonSelectList { get; set; } = default!;
    
    /// <summary>
    /// Select list for storage rooms.
    /// </summary>
    [ValidateNever]
    public SelectList StorageRoomSelectList { get; set; } = default!;
}