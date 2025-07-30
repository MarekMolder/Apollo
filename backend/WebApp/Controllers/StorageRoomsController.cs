using App.BLL.Contracts;
using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class StorageRoomsController : Controller
    {
        private readonly IAppBll _bll;

        public StorageRoomsController(IAppBll bll)
        {
            _bll = bll;
        }

        // GET: StorageRooms
        public async Task<IActionResult> Index()
        {
            var res = await _bll.StorageRoomService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: StorageRooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: StorageRooms/Create
        public async Task<IActionResult> Create()
        {
            var vm = new StorageRoomCreateEditViewModel
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                    nameof(Address.Id),
                    nameof(Address.Name)
                ),
            };
            
            return View(vm);
        }

        // POST: StorageRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StorageRoomCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.StorageRoom.EndedAt.HasValue)
                {
                    vm.StorageRoom.EndedAt = DateTime.SpecifyKind(vm.StorageRoom.EndedAt.Value, DateTimeKind.Utc);
                }
                
                if (!string.IsNullOrWhiteSpace(vm.RolesInput))
                {
                    vm.StorageRoom.AllowedRoles = vm.RolesInput
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .ToList();
                }
                
                _bll.StorageRoomService.Add(vm.StorageRoom);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                nameof(Address.Id), nameof(Address.Name), vm.StorageRoom.AddressId);

            return View(vm);
        }

        // GET: StorageRooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            if (inventory == null)
            {
                return NotFound();
            }
            
            var vm = new StorageRoomCreateEditViewModel
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                    nameof(Address.Id),
                    nameof(Address.Name),
                    inventory.AddressId
                ),
                
                StorageRoom = inventory,
                
                RolesInput = inventory.AllowedRoles != null ? string.Join(",", inventory.AllowedRoles) : ""
            };
            return View(vm);
        }

        // POST: StorageRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StorageRoomCreateEditViewModel vm)
        {
            if (id != vm.StorageRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (vm.StorageRoom.EndedAt.HasValue)
                {
                    vm.StorageRoom.EndedAt = DateTime.SpecifyKind(vm.StorageRoom.EndedAt.Value, DateTimeKind.Utc);
                }
                
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

            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                nameof(Address.Id), nameof(Address.Name), vm.StorageRoom.AddressId);

            return View(vm);
        }

        // GET: StorageRooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: StorageRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.StorageRoomService.RemoveAsync(id, User.GetUserId());

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
