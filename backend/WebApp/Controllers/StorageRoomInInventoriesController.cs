using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;
using IAppBLL = App.BLL.Contracts.IAppBLL;

namespace WebApp.Controllers
{
    [Authorize]
    public class StorageRoomInInventoriesController : Controller
    {
        private readonly IAppBLL _bll;

        public StorageRoomInInventoriesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: StorageRoomInInventories
        public async Task<IActionResult> Index()
        {
            var res = await _bll.StorageRoomInInventoryService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: StorageRoomInInventories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.StorageRoomInInventoryService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: StorageRoomInInventories/Create
        public async Task<IActionResult> Create()
        {
            var vm = new StorageRoomInInventoryCreateEditViewModel
            {
                InventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                    nameof(Inventory.Id),
                    nameof(Inventory.Name)
                ),

                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name)
                ),
            };
            
            return View(vm);
        }

        // POST: StorageRoomInInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StorageRoomInInventoryCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.StorageRoomInInventory.EndedAt.HasValue)
                { 
                    vm.StorageRoomInInventory.EndedAt = DateTime.SpecifyKind(vm.StorageRoomInInventory.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.StorageRoomInInventoryService.Add(vm.StorageRoomInInventory);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.InventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                nameof(Inventory.Id), nameof(Inventory.Name), vm.StorageRoomInInventory.InventoryId);
            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.StorageRoomInInventory.StorageRoomId);

            return View(vm);
        }

        // GET: StorageRoomInInventories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageRoomInInventory = await _bll.StorageRoomInInventoryService.FindAsync(id.Value, User.GetUserId());
            if (storageRoomInInventory == null)
            {
                return NotFound();
            }
            
            var vm = new StorageRoomInInventoryCreateEditViewModel
            {
                InventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                    nameof(Inventory.Id),
                    nameof(Inventory.Name),
                    storageRoomInInventory.InventoryId
                ),

                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    storageRoomInInventory.StorageRoomId
                ),
                StorageRoomInInventory = storageRoomInInventory
            };
            return View(vm);
        }

        // POST: StorageRoomInInventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StorageRoomInInventoryCreateEditViewModel vm)
        {
            if (id != vm.StorageRoomInInventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (vm.StorageRoomInInventory.EndedAt.HasValue)
                { 
                    vm.StorageRoomInInventory.EndedAt = DateTime.SpecifyKind(vm.StorageRoomInInventory.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.StorageRoomInInventoryService.Update(vm.StorageRoomInInventory);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.InventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                nameof(Inventory.Id), nameof(Inventory.Name), vm.StorageRoomInInventory.InventoryId);
            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.StorageRoomInInventory.StorageRoomId);

            return View(vm);
        }

        // GET: StorageRoomInInventories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var action = await _bll.StorageRoomInInventoryService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                return NotFound();
            }

            return View(action);
        }

        // POST: StorageRoomInInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.StorageRoomInInventoryService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
