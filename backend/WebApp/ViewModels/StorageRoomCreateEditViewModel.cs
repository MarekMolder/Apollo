using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class StorageRoomCreateEditViewModel
{
    public StorageRoom StorageRoom { get; set; } = default!;

    [ValidateNever]
    public SelectList AddressSelectList { get; set; } = default!;
    
    [ValidateNever]
    public string? RolesInput { get; set; }
}