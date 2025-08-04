using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

/// <summary>
/// ViewModel for creating and editing an Supplier.
/// Provides select lists for dropdowns in the UI.
/// </summary>
public class SupplierCreateEditViewModel
{
    /// <summary>
    /// The Supplier entity being created or edited.
    /// </summary>
    public Supplier Supplier { get; set; } = default!;

    /// <summary>
    /// Select list of addresses used to populate an address dropdown menu.
    /// </summary>
    [ValidateNever]
    public SelectList AddressSelectList { get; set; } = default!;
}