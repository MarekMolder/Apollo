using App.BLL.Contracts;
using App.BLL.DTO;
using App.Domain.Enums;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing ActionEntity records.
    /// </summary>
    [Authorize]
    public class ActionTypesController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ActionTypesController> _logger;
        
        public ActionTypesController(IAppBll bll, ILogger<ActionTypesController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Display all action types.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("User {User} accessing ActionType list", User.Identity?.Name);
            var res = await _bll.ActionTypeEntityService.AllAsync(User.GetUserId());
            return View(res);
        }

        /// <summary>
        /// View details of a specific action type.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details view failed: id is null");
                return NotFound();
            }

            var entity = await _bll.ActionTypeEntityService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                _logger.LogWarning("ActionType with id {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Show create action type form.
        /// </summary>
        public IActionResult Create()
        {
            _logger.LogInformation("User {User} is accessing Create ActionType view", User.Identity?.Name);
            PopulateEnumSelectList();
            return View();
        }
        /// <summary>
        /// Save new action type.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActionTypeEntity actionTypeEntity)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating new ActionType: {Name}", actionTypeEntity.Name);
                _bll.ActionTypeEntityService.Add(actionTypeEntity);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Create ActionType form invalid");
            PopulateEnumSelectList();
            return View(actionTypeEntity);
        }

        /// <summary>
        /// Show edit form for action type.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit view failed: id is null");
                return NotFound();
            }
            
            var entity = await _bll.ActionTypeEntityService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                _logger.LogWarning("Edit ActionType failed: id {Id} not found", id);
                return NotFound();
            }

            PopulateEnumSelectList();
            return View(entity);
        }

        /// <summary>
        /// Save changes to action type.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ActionTypeEntity actionTypeEntity)
        {
            if (id != actionTypeEntity.Id)
            {
                _logger.LogWarning("Edit ActionType failed: mismatched id {Id}", id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating ActionType with id {Id}", id);
                _bll.ActionTypeEntityService.Update(actionTypeEntity);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _logger.LogWarning("Edit ActionType form invalid for id {Id}", id);
            PopulateEnumSelectList();
            return View(actionTypeEntity);
        }

        /// <summary>
        /// Show confirmation for deleting an action type.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete view failed: id is null");
                return NotFound();
            }
            
            var entity = await _bll.ActionTypeEntityService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                _logger.LogWarning("Delete view failed: ActionType with id {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Delete action type after confirmation.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting ActionType with id {Id}", id);
            await _bll.ActionTypeEntityService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// Populate enum select list for ActionTypeEnum.
        /// </summary>
        private void PopulateEnumSelectList()
        {
            ViewData["ActionTypeEnumList"] = new SelectList(
                Enum.GetValues(typeof(ActionTypeEnum))
                    .Cast<ActionTypeEnum>()
                    .Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.ToString()
                    }),
                "Value",
                "Text"
            );
        }
    }
}
