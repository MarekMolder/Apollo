using App.BLL.Contracts;
using App.DAL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    /// <summary>
    /// Controller for managing ActionEntity records.
    /// </summary>
    [Authorize]
    public class ActionsController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ActionsController> _logger;
        
        public ActionsController(IAppBll bll, ILogger<ActionsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Displays a list of all actions for the current user.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all actions for user {UserId}", User.GetUserId());
            var res = await _bll.ActionEntityService.AllAsync(User.GetUserId());
            return View(res);
        }

        /// <summary>
        /// Shows the details of a specific ActionEntity.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.ActionEntityService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                _logger.LogWarning("Action with ID {Id} not found", id);
                return NotFound();
            }
            
            return View(entity);
        }

        /// <summary>
        /// Displays the create form for a new ActionEntity.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Loading create action form for user {UserId}", User.GetUserId());
            
            var vm = new ActionEntityCreateEditViewModel
            {
                ActionTypeSelectList = new SelectList(await _bll.ActionTypeEntityService.AllAsync(User.GetUserId()),
                    nameof(ActionTypeEntity.Id),
                    nameof(ActionTypeEntity.Name)
                ),

                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),

                ReasonSelectList = new SelectList(await _bll.ReasonService.AllAsync(User.GetUserId()),
                    nameof(Reason.Id),
                    nameof(Reason.Description)
                ),
                
                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id),
                nameof(StorageRoom.Name)
                ),
            };
            
            return View(vm);
        }

        /// <summary>
        /// Handles the form submission to create a new ActionEntity.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActionEntityCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating new action for user {UserId}", User.GetUserId());
                _bll.ActionEntityService.Add(vm.ActionEntity, User.GetUserId());
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _logger.LogWarning("Invalid model state while creating action for user {UserId}", User.GetUserId());

            vm.ActionTypeSelectList = new SelectList(await _bll.ActionTypeEntityService.AllAsync(User.GetUserId()),
                nameof(ActionTypeEntity.Id), nameof(ActionTypeEntity.Name), vm.ActionEntity.ActionTypeId);
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.ActionEntity.ProductId);
            vm.ReasonSelectList = new SelectList(await _bll.ReasonService.AllAsync(User.GetUserId()),
                nameof(Reason.Id), nameof(Reason.Description), vm.ActionEntity.ReasonId);
            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.ActionEntity.StorageRoomId);

            return View(vm);
        }

        /// <summary>
        /// Displays the edit form for a specific ActionEntity.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }

            var actionEntity = await _bll.ActionEntityService.FindAsync(id.Value, User.GetUserId());
            if (actionEntity == null)
            {
                _logger.LogWarning("Action with ID {Id} not found for edit", id);
                return NotFound();
            }
            
            var vm = new ActionEntityCreateEditViewModel
            {
                ActionTypeSelectList = new SelectList(await _bll.ActionTypeEntityService.AllAsync(User.GetUserId()),
                    nameof(ActionTypeEntity.Id),
                    nameof(ActionTypeEntity.Name),
                    actionEntity.ActionTypeId
                ),

                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name),
                    actionEntity.ProductId
                ),

                ReasonSelectList = new SelectList(await _bll.ReasonService.AllAsync(User.GetUserId()),
                    nameof(Reason.Id),
                    nameof(Reason.Description),
                    actionEntity.ReasonId
                ),
                
                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    actionEntity.StorageRoomId
                ),
                ActionEntity = actionEntity
            };
            return View(vm);
        }

        /// <summary>
        /// Handles the form submission to update an ActionEntity.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ActionEntityCreateEditViewModel vm)
        {
            if (id != vm.ActionEntity.Id)
            {
                _logger.LogWarning("Edit mismatch ID {Id} vs {EntityId}", id, vm.ActionEntity.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating action with ID {Id} for user {UserId}", id, User.GetUserId());
                _bll.ActionEntityService.Update(vm.ActionEntity);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _logger.LogWarning("Invalid model state while editing action {Id}", id);

            vm.ActionTypeSelectList = new SelectList(await _bll.ActionTypeEntityService.AllAsync(User.GetUserId()),
                nameof(ActionTypeEntity.Id), nameof(ActionTypeEntity.Name), vm.ActionEntity.ActionTypeId);
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.ActionEntity.ProductId);
            vm.ReasonSelectList = new SelectList(await _bll.ReasonService.AllAsync(User.GetUserId()),
                nameof(Reason.Id), nameof(Reason.Description), vm.ActionEntity.ReasonId);
            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.ActionEntity.StorageRoomId);

            return View(vm);
        }
        
        /// <summary>
        /// Displays the delete confirmation view for a specific ActionEntity.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }

            var action = await _bll.ActionEntityService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                _logger.LogWarning("Action with ID {Id} not found for delete", id);
                return NotFound();
            }

            return View(action);
        }

        /// <summary>
        /// Permanently deletes the specified ActionEntity.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting action with ID {Id} for user {UserId}", id, User.GetUserId());
            await _bll.ActionEntityService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
