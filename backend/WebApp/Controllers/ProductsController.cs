using App.BLL.Contracts;
using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing products.
    /// </summary>
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IAppBll bll, ILogger<ProductsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Display all products.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all products for user {UserId}", User.GetUserId());
            var res = await _bll.ProductService.AllAsync(User.GetUserId());
            return View(res);
        }

        /// <summary>
        /// Display product details by ID.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.ProductService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                _logger.LogWarning("Product with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Show create form.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Opening create form for product");
            var vm = new ProductCreateEditViewModel
            {
                ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                    nameof(ProductCategory.Id),
                    nameof(ProductCategory.Name)
                ),
            };
            
            return View(vm);
        }

        /// <summary>
        /// Handle POST to create product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating product {Name}", vm.Product.Name);
                _bll.ProductService.Add(vm.Product);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _logger.LogWarning("Invalid model state on product creation");
            vm.ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                nameof(ProductCategory.Id), nameof(ProductCategory.Name), vm.Product.ProductCategoryId);

            return View(vm);
        }

        /// <summary>
        /// Show edit form.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }

            var product = await _bll.ProductService.FindAsync(id.Value, User.GetUserId());
            if (product == null)
            {
                _logger.LogWarning("Product with ID {Id} not found for edit", id);
                return NotFound();
            }
            
            var vm = new ProductCreateEditViewModel
            {
                ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                    nameof(ProductCategory.Id),
                    nameof(ProductCategory.Name),
                    product.ProductCategoryId
                ),
                Product = product
            };
            return View(vm);
        }

        /// <summary>
        /// Handle POST to update product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductCreateEditViewModel vm)
        {
            if (id != vm.Product.Id)
            {
                _logger.LogWarning("Edit ID mismatch: {PostedId} != {EntityId}", id, vm.Product.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating product {Id}", id);
                _bll.ProductService.Update(vm.Product);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while editing product {Id}", id);
            vm.ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                nameof(ProductCategory.Id), nameof(ProductCategory.Name), vm.Product.ProductCategory);

            return View(vm);
        }

        /// <summary>
        /// Show delete confirmation page.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }

            var action = await _bll.ProductService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                _logger.LogWarning("Product with ID {Id} not found for delete", id);
                return NotFound();
            }

            return View(action);
        }

        /// <summary>
        /// Handle POST to delete product.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting product {Id}", id);
            await _bll.ProductService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
