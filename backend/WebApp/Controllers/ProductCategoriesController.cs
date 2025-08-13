using App.BLL.Contracts;
using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing product categories.
    /// </summary>
    [Authorize]
    public class ProductCategoriesController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ProductCategoriesController> _logger;
        
        public ProductCategoriesController(IAppBll bll, ILogger<ProductCategoriesController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Display all product categories.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all product categories for user {UserId}", User.GetUserId());
            var res = await _bll.ProductCategoryService.AllAsync(User.GetUserId());
            return View(res);
        }

        /// <summary>
        /// Display details of a specific product category.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.ProductCategoryService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                _logger.LogWarning("ProductCategory with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Show form to create a new product category.
        /// </summary>
        public IActionResult Create()
        {
            _logger.LogInformation("Opening create product category form");
            return View();
        }

        /// <summary>
        /// Handle POST request to create a new product category.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating new product category");
                _bll.ProductCategoryService.Add(productCategory);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while creating product category");
            return View(productCategory);
        }

        /// <summary>
        /// Show form to edit a product category.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }
            
            var entity = await _bll.ProductCategoryService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                _logger.LogWarning("ProductCategory with ID {Id} not found for edit", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Handle POST request to update a product category.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                _logger.LogWarning("Edit ID mismatch: {PostedId} != {EntityId}", id, productCategory.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating product category ID {Id}", id);
                _bll.ProductCategoryService.Update(productCategory);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while editing product category ID {Id}", id);
            return View(productCategory);
        }

        /// <summary>
        /// Show confirmation view for deleting a product category.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }


            var entity = await _bll.ProductCategoryService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                _logger.LogWarning("ProductCategory with ID {Id} not found for delete", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Handle POST request to delete a product category.
        /// </summary>        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting product category ID {Id}", id);
            await _bll.ProductCategoryService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
