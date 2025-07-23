using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class ActionEntityCreateEditViewModel
{
    public ActionEntity ActionEntity { get; set; } = default!;

    [ValidateNever]
    public SelectList ActionTypeSelectList { get; set; } = default!;

    [ValidateNever]
    public SelectList ProductSelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList? ReasonSelectList { get; set; } = default!;

    [ValidateNever]
    public SelectList? StockAuditSelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList? SupplierSelectList { get; set; } = default!;
    
    
    [ValidateNever]
    public SelectList? StorageRoomSelectList { get; set; } = default!;
}