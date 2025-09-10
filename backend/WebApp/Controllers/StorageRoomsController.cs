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
    /// Controller for managing storage rooms.
    /// </summary>
    [Authorize]
    public class StorageRoomsController : Controller
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<StorageRoomsController> _logger;
        
        public StorageRoomsController(IAppBll bll, ILogger<StorageRoomsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Displays all storage rooms.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all storage rooms for user {UserId}", User.GetUserId());
            var res = await _bll.StorageRoomService.AllAsync();
            return View(res);
        }

        /// <summary>
        /// Displays details for a specific storage room.
        /// </summary>
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details called with null ID");
                return NotFound();
            }

            var entity = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                _logger.LogWarning("Storage room with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Shows form to create a new storage room.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("Opening create form for storage room");

            var vm = new StorageRoomCreateEditViewModel
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                    nameof(Address.Id),
                    nameof(Address.Name)
                ),
            };
            
            return View(vm);
        }

        /// <summary>
        /// Processes creation of a new storage room.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StorageRoomCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Creating storage room for user {UserId}", User.GetUserId());
                
                if (!string.IsNullOrWhiteSpace(vm.RolesInput))
                {
                    vm.StorageRoom.AllowedRoles = vm.RolesInput
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .ToList();
                }
                
                _logger.LogWarning("Invalid model state while creating storage room");
                _bll.StorageRoomService.Add(vm.StorageRoom);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                nameof(Address.Id), nameof(Address.Name), vm.StorageRoom.AddressId);

            return View(vm);
        }

        /// <summary>
        /// Shows form to edit an existing storage room.
        /// </summary>
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit called with null ID");
                return NotFound();
            }

            var storageRoom = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            if (storageRoom == null)
            {
                _logger.LogWarning("Storage room with ID {Id} not found", id);
                return NotFound();
            }
            
            var vm = new StorageRoomCreateEditViewModel
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                    nameof(Address.Id),
                    nameof(Address.Name),
                    storageRoom.AddressId
                ),
                
                StorageRoom = storageRoom,
                
                RolesInput = storageRoom.AllowedRoles != null ? string.Join(",", storageRoom.AllowedRoles) : ""
            };
            return View(vm);
        }

        /// <summary>
        /// Processes editing of a storage room.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StorageRoomCreateEditViewModel vm)
        {
            if (id != vm.StorageRoom.Id)
            {
                _logger.LogWarning("Edit mismatch ID {PostedId} vs {EntityId}", id, vm.StorageRoom.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Updating storage room with ID {Id}", id);
                
                if (!string.IsNullOrWhiteSpace(vm.RolesInput))
                {
                    vm.StorageRoom.AllowedRoles = vm.RolesInput
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .ToList();
                }
                else
                {
                    vm.StorageRoom.AllowedRoles = new List<string>();
                }
                
                _bll.StorageRoomService.Update(vm.StorageRoom);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Invalid model state while editing storage room {Id}", id);
            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                nameof(Address.Id), nameof(Address.Name), vm.StorageRoom.AddressId);

            return View(vm);
        }

        /// <summary>
        /// Shows confirmation page to delete a storage room.
        /// </summary>
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete called with null ID");
                return NotFound();
            }
            
            var entity = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                _logger.LogWarning("Storage room with ID {Id} not found", id);
                return NotFound();
            }

            return View(entity);
        }

        /// <summary>
        /// Processes confirmed deletion of a storage room.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation("Deleting storage room with ID {Id}", id);
            await _bll.StorageRoomService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
