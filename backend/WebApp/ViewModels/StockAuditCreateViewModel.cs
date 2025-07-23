using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class StockAuditCreateViewModel
{
    public StockAudit StockAudit { get; set; } = default!;

    [ValidateNever]
    public SelectList StorageRoomSelectList { get; set; } = default!;
}