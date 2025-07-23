using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class StorageRoomInInventoryCreateEditViewModel
{
    public StorageRoomInInventory StorageRoomInInventory { get; set; } = default!;

    [ValidateNever]
    public SelectList InventorySelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList StorageRoomSelectList { get; set; } = default!;
}