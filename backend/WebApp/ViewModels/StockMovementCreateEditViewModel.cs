using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class StockMovementCreateEditViewModel
{
    public StockMovement StockMovement { get; set; } = default!;

    [ValidateNever]
    public SelectList FromInventorySelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList FromStorageRoomSelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList ProductSelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList ToInventorySelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList ToStorageRoomSelectList { get; set; } = default!;
}