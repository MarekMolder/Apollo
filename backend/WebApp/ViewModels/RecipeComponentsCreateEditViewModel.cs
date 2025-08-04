using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

/// <summary>
/// ViewModel for creating and editing an RecipeComponent.
/// Provides select lists for dropdowns in the UI.
/// </summary>
public class RecipeComponentsCreateEditViewModel
{
    /// <summary>
    /// The RecipeComponent entity being created or edited.
    /// </summary>
    public RecipeComponent RecipeComponent { get; set; } = default!;

    /// <summary>
    /// Select list of all products that can be used as recipes (main product).
    /// </summary>
    [ValidateNever]
    public SelectList ProductRecipeSelectList { get; set; } = default!;
    
    /// <summary>
    /// Select list of products that can be used as components (ingredients) in a recipe.
    /// </summary>
    [ValidateNever]
    public SelectList ComponentProductSelectList { get; set; } = default!;
}