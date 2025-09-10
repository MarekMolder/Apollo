using App.BLL.Contracts;
using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing Address records.
    /// </summary>
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<AddressesController> _logger;

        public AddressesController(IAppBll bll, ILogger<AddressesController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Displays a list of all addresses for the current user.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all addresses for user {UserId}", User.GetUserId());
            var res = await _bll.AddressService.AllAsync();
            return View(res);
        }

        /// <summary>
        /// Displays the details of a specific address.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.AddressService.FindAsync(id.Value);
            
            if (entity == null)
            {
                _logger.LogWarning("Address with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Displays the form to create a new address.
        /// </summary>
        public IActionResult Create()
        {
            _logger.LogInformation("Loading address creation form for user {UserId}", User.GetUserId());
            return View();
        }

        /// <summary>
        /// Handles the creation of a new address.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating new address for user {UserId}", User.GetUserId());
                _bll.AddressService.Add(address);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while creating address for user {UserId}", User.GetUserId());
            return View(address);
        }

        /// <summary>
        /// Displays the edit form for a specific address.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }

            var entity = await _bll.AddressService.FindAsync(id.Value);
            if (entity == null)
            {
                _logger.LogWarning("Address with ID {Id} not found for edit", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Handles the update of an existing address.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Address address)
        {
            if (id != address.Id)
            {
                _logger.LogWarning("Edit mismatch ID {Id} vs {AddressId}", id, address.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating address with ID {Id} for user {UserId}", id, User.GetUserId());
                _bll.AddressService.Update(address);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while editing address {Id}", id);
            return View(address);
        }

        /// <summary>
        /// Displays the delete confirmation view for a specific address.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }

            var entity = await _bll.AddressService.FindAsync(id.Value);
            if (entity == null)
            {
                _logger.LogWarning("Address with ID {Id} not found for delete", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Handles the deletion of an address.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting address with ID {Id} for user {UserId}", id, User.GetUserId());
            await _bll.AddressService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
