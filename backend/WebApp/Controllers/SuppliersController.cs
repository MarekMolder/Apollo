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
    /// Controller for managing suppliers.
    /// </summary>
    [Authorize]
    public class SuppliersController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<SuppliersController> _logger;
        
        public SuppliersController(IAppBll bll, ILogger<SuppliersController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Displays list of all suppliers.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all suppliers for user {UserId}", User.GetUserId());
            var res = await _bll.SupplierService.AllAsync();
            return View(res);
        }

        /// <summary>
        /// Displays details for a specific supplier.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.SupplierService.FindAsync(id.Value);
            
            if (entity == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Shows form to create a new supplier.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Opening create form for supplier");

            var vm = new SupplierCreateEditViewModel
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(),
                    nameof(Address.Id),
                    nameof(Address.Name)
                )
            };
            
            return View(vm);
        }

        /// <summary>
        /// Processes the creation of a new supplier.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating new supplier for user {UserId}", User.GetUserId());
                _bll.SupplierService.Add(vm.Supplier);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while creating supplier");
            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(),
                nameof(Address.Id), nameof(Address.Name), vm.Supplier.AddressId);
            
            return View(vm);
        }

        /// <summary>
        /// Shows form to edit an existing supplier.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }

            var supplier = await _bll.SupplierService.FindAsync(id.Value);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found", id);
                return NotFound();
            }
            
            var vm = new SupplierCreateEditViewModel
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(),
                    nameof(Address.Id),
                    nameof(Address.Name),
                    supplier.AddressId
                ),
                Supplier = supplier
            };
            return View(vm);
        }

        /// <summary>
        /// Processes the update of an existing supplier.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SupplierCreateEditViewModel vm)
        {
            if (id != vm.Supplier.Id)
            {
                _logger.LogWarning("Edit mismatch ID {PostedId} vs {EntityId}", id, vm.Supplier.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating supplier with ID {Id} for user {UserId}", id, User.GetUserId());
                _bll.SupplierService.Update(vm.Supplier);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while editing supplier {Id}", id);
            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(),
                nameof(Address.Id), nameof(Address.Name), vm.Supplier.AddressId);

            return View(vm);
        }

        /// <summary>
        /// Shows confirmation view to delete a supplier.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }

            var supplier = await _bll.SupplierService.FindAsync(id.Value);

            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found", id);
                return NotFound();
            }

            return View(supplier);
        }

        /// <summary>
        /// Processes confirmed deletion of a supplier.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting supplier with ID {Id} for user {UserId}", id, User.GetUserId());
            await _bll.SupplierService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
