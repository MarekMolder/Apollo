using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

/// <summary>
/// ViewModel for creating and editing an Product.
/// Provides select lists for dropdowns in the UI.
/// </summary>
public class ProductCreateEditViewModel
{
    /// <summary>
    /// The Product entity being created or edited.
    /// </summary>
    public Product Product { get; set; } = default!;

    /// <summary>
    /// A select list of product categories used to populate the category dropdown in the form.
    /// </summary>
    [ValidateNever]
    public SelectList ProductCategorySelectList { get; set; } = default!;
    
    /// <summary>
    /// A select list of suppliers used to populate the category dropdown in the form.
    /// </summary>
    [ValidateNever]
    public SelectList SupplierSelectList { get; set; } = default!;
}