using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class RecipeComponentsCreateEditViewModel
{
    public RecipeComponent RecipeComponent { get; set; } = default!;

    [ValidateNever]
    public SelectList ProductRecipeSelectList { get; set; } = default!;
    
    [ValidateNever]
    public SelectList? ComponentProductSelectList { get; set; } = default!;
}