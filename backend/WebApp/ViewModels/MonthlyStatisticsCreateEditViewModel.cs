using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

/// <summary>
/// ViewModel for creating and editing an MonthlyStatistics.
/// Provides select lists for dropdowns in the UI.
/// </summary>
public class MonthlyStatisticsCreateEditViewModel
{
    /// <summary>
    /// The main MonthlyStatistics entity being created or edited.
    /// </summary>
    public MonthlyStatistics MonthlyStatistics { get; set; } = default!;

    /// <summary>
    /// Select list of available products to associate with the monthly statistic.
    /// </summary>
    [ValidateNever]
    public SelectList ProductSelectList { get; set; } = default!;
    
    /// <summary>
    /// Select list of available productCategories to associate with the monthly statistic.
    /// </summary>
    [ValidateNever]
    public SelectList ProductCategorySelectList { get; set; } = default!;
    
    /// <summary>
    /// Select list of available storage rooms for selection.
    /// </summary>
    [ValidateNever]
    public SelectList StorageRoomSelectList { get; set; } = default!;
}