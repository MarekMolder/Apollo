using App.BLL.Contracts;
using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing reasons.
    /// </summary>
    [Authorize]
    public class ReasonsController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ReasonsController> _logger;
        
        public ReasonsController(IAppBll bll, ILogger<ReasonsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Displays list of all reasons.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all reasons for user {UserId}", User.GetUserId());
            var res = await _bll.ReasonService.AllAsync();
            return View(res);
        }

        /// <summary>
        /// Shows reason details by ID.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.ReasonService.FindAsync(id.Value);
            
            if (entity == null)
            {
                _logger.LogWarning("Reason with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Shows form for creating a new reason.
        /// </summary>
        public IActionResult Create()
        {
            _logger.LogInformation("Opening create reason form");
            return View();
        }

        /// <summary>
        /// Handles form post for reason creation.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reason reason)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating reason: {Description}", reason.Description);
                _bll.ReasonService.Add(reason);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while creating reason");
            return View(reason);
        }

        /// <summary>
        /// Shows form to edit an existing reason.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }
            
            var entity = await _bll.ReasonService.FindAsync(id.Value);
            if (entity == null)
            {
                _logger.LogWarning("Reason with ID {Id} not found for edit", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Handles form post for updating an existing reason.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Reason reason)
        {
            if (id != reason.Id)
            {
                _logger.LogWarning("Edit mismatch ID {PostedId} vs {EntityId}", id, reason.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating reason with ID {Id}", id);
                _bll.ReasonService.Update(reason);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while editing reason {Id}", id);
            return View(reason);
        }

        /// <summary>
        /// Shows confirmation page for deleting a reason.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }

            var entity = await _bll.ReasonService.FindAsync(id.Value);
            if (entity == null)
            {
                _logger.LogWarning("Reason with ID {Id} not found for delete", id);
                return NotFound();
            }

            return View(entity);
        }
        
        /// <summary>
        /// Handles confirmed deletion of a reason.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting reason with ID {Id}", id);
            await _bll.ReasonService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
