using App.BLL.Contracts;
using App.DTO.v1;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;
using ProductCategory = App.Domain.Logic.ProductCategory;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing monthly statistics.
    /// </summary>
    [Authorize]
    public class MonthlyStatisticsController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<MonthlyStatisticsController> _logger;

        public MonthlyStatisticsController(IAppBll bll, ILogger<MonthlyStatisticsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Display list of all monthly statistics.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all monthly statistics for user {UserId}", User.GetUserId());
            var res = await _bll.MonthlyStatisticsService.AllAsync();
            return View(res);
        }

        /// <summary>
        /// Display details of a specific monthly statistics record.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.MonthlyStatisticsService.FindAsync(id.Value,User.GetUserId());
            
            if (entity == null)
            {
                _logger.LogWarning("MonthlyStatistics with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Show form for creating a new monthly statistics record.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Opening create form for monthly statistics");

            var vm = new MonthlyStatisticsCreateEditViewModel()
            {
                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),
                
                ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                    nameof(ProductCategory.Id),
                    nameof(ProductCategory.Name)
                ),

                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name)
                ),
            };
            
            return View(vm);
        }

        /// <summary>
        /// Handle POST request to create a new monthly statistics record.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonthlyStatisticsCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating new monthly statistics record for user {UserId}", User.GetUserId());
                _bll.MonthlyStatisticsService.Add(vm.MonthlyStatistics, User.GetUserId());
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _logger.LogWarning("Invalid model state while creating monthly statistics");
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.MonthlyStatistics.ProductId);
            vm.ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                nameof(ProductCategory.Id), nameof(ProductCategory.Name), vm.MonthlyStatistics.ProductCategoryId);
            vm.StorageRoomSelectList = new SelectList(await _bll.MonthlyStatisticsService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.MonthlyStatistics.StorageRoomId);
            
            return View(vm);
        }

        /// <summary>
        /// Show form for editing a specific monthly statistics record.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }

            var monthlyStatistics = await _bll.MonthlyStatisticsService.FindAsync(id.Value, User.GetUserId());
            if (monthlyStatistics == null)
            {
                _logger.LogWarning("MonthlyStatistics with ID {Id} not found for edit", id);
                return NotFound();
            }
            
            var vm = new MonthlyStatisticsCreateEditViewModel()
            {
                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name),
                    monthlyStatistics.ProductId
                ),
                
                ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                    nameof(ProductCategory.Id),
                    nameof(ProductCategory.Name),
                    monthlyStatistics.ProductCategoryId
                ),
                
                StorageRoomSelectList = new SelectList(await _bll.MonthlyStatisticsService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    monthlyStatistics.StorageRoomId
                ),
                
                MonthlyStatistics = monthlyStatistics
            };
            return View(vm);
        }

        /// <summary>
        /// Handle POST request to update a monthly statistics record.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MonthlyStatisticsCreateEditViewModel vm)
        {
            if (id != vm.MonthlyStatistics.Id)
            {
                _logger.LogWarning("Edit ID mismatch: {PostedId} != {EntityId}", id, vm.MonthlyStatistics.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating monthly statistics ID {Id}", id);
                _bll.MonthlyStatisticsService.Update(vm.MonthlyStatistics);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _logger.LogWarning("Invalid model state while editing monthly statistics");

            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.MonthlyStatistics.ProductId);
            vm.ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                nameof(ProductCategory.Id), nameof(ProductCategory.Name), vm.MonthlyStatistics.ProductCategoryId);
            vm.StorageRoomSelectList = new SelectList(await _bll.RecipeComponentService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.MonthlyStatistics.StorageRoomId);

            return View(vm);
        }

        /// <summary>
        /// Show delete confirmation view for a monthly statistics record.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }

            var monthlyStatistics = await _bll.MonthlyStatisticsService.FindAsync(id.Value, User.GetUserId());

            if (monthlyStatistics == null)
            {
                _logger.LogWarning("MonthlyStatistics with ID {Id} not found for delete", id);
                return NotFound();
            }

            return View(monthlyStatistics);
        }

        /// <summary>
        /// Handle POST request to delete a monthly statistics record.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting monthly statistics ID {Id}", id);
            await _bll.MonthlyStatisticsService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
