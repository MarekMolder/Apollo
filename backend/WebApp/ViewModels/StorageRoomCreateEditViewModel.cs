using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

/// <summary>
/// ViewModel for creating and editing an StorageRoom.
/// Provides select lists for dropdowns in the UI.
/// </summary>
public class StorageRoomCreateEditViewModel
{
    /// <summary>
    /// The StorageRoom entity being created or edited.
    /// </summary>
    public StorageRoom StorageRoom { get; set; } = default!;

    /// <summary>
    /// Select list of addresses for populating a dropdown in the UI.
    /// </summary>
    [ValidateNever]
    public SelectList AddressSelectList { get; set; } = default!;
    
    /// <summary>
    /// Comma-separated string input for allowed user roles.
    /// Parsed and stored in StorageRoom.AllowedRoles list.
    /// </summary>
    [ValidateNever]
    public string? RolesInput { get; set; }
}