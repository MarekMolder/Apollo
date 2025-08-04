using App.BLL.Contracts;
using App.DTO.v1;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing recipe components in the system.
    /// </summary>
    [Authorize(Roles = "admin")]
    public class RecipeComponentsController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<RecipeComponentsController> _logger;

        public RecipeComponentsController(IAppBll bll, ILogger<RecipeComponentsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Displays list of all recipe components.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all recipe components for user {UserId}", User.GetUserId());
            var res = await _bll.RecipeComponentService.AllAsync(User.GetUserId());
            return View(res);
        }

        /// <summary>
        /// Displays details for a specific recipe component.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.RecipeComponentService.FindAsync(id.Value,User.GetUserId());
            
            if (entity == null)
            {
                _logger.LogWarning("Recipe component with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Shows form to create a new recipe component.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Opening create form for recipe component");

            var vm = new RecipeComponentsCreateEditViewModel()
            {
                ProductRecipeSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),

                ComponentProductSelectList = new SelectList(
                    (await _bll.ProductService.AllAsync(User.GetUserId()))
                    .Where(p => p.IsComponent),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),
            };
            
            return View(vm);
        }

        /// <summary>
        /// Processes the creation of a new recipe component.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeComponentsCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating recipe component for user {UserId}", User.GetUserId());
                _bll.RecipeComponentService.Add(vm.RecipeComponent, User.GetUserId());
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _logger.LogWarning("Invalid model state while creating recipe component");
            
            vm.ProductRecipeSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.RecipeComponent.ProductRecipeId);
            vm.ComponentProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.RecipeComponent.ComponentProductId);
            
            return View(vm);
        }

        /// <summary>
        /// Shows form to edit an existing recipe component.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }

            var recipeComponent = await _bll.RecipeComponentService.FindAsync(id.Value, User.GetUserId());
            if (recipeComponent == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }
            
            var allProducts = await _bll.ProductService.AllAsync(User.GetUserId());
            
            var vm = new RecipeComponentsCreateEditViewModel()
            {
                ProductRecipeSelectList = new SelectList(
                    allProducts,
                    nameof(Product.Id),
                    nameof(Product.Name),
                    recipeComponent.ProductRecipeId
                ),
                
                ComponentProductSelectList = new SelectList(
                    allProducts.Where(p => p.IsComponent),
                    nameof(Product.Id),
                    nameof(Product.Name),
                    recipeComponent.ComponentProductId
                ),
                
                RecipeComponent = recipeComponent
            };
            return View(vm);
        }

        /// <summary>
        /// Processes the update of an existing recipe component.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RecipeComponentsCreateEditViewModel vm)
        {
            if (id != vm.RecipeComponent.Id)
            {
                _logger.LogWarning("Edit mismatch ID {PostedId} vs {EntityId}", id, vm.RecipeComponent.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating recipe component with ID {Id}", id);
                _bll.RecipeComponentService.Update(vm.RecipeComponent);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _logger.LogWarning("Invalid model state while editing recipe component {Id}", id);
            vm.ProductRecipeSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.RecipeComponent.ProductRecipeId);
            vm.ComponentProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.RecipeComponent.ComponentProductId);

            return View(vm);
        }

        /// <summary>
        /// Shows confirmation view to delete a recipe component.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }

            var recipeComponent = await _bll.RecipeComponentService.FindAsync(id.Value, User.GetUserId());

            if (recipeComponent == null)
            {
                _logger.LogWarning("Recipe component with ID {Id} not found for delete", id);
                return NotFound();
            }

            return View(recipeComponent);
        }

        /// <summary>
        /// Processes confirmed deletion of a recipe component.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting recipe component with ID {Id}", id);
            await _bll.RecipeComponentService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}