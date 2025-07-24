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
    public class InventoriesController : Controller
    {
        private readonly IAppBLL _bll;

        public InventoriesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            var res = await _bll.InventoryService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.InventoryService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Inventories/Create
        public async Task<IActionResult> Create()
        {
            var vm = new InventoryCreateEditViewModel
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                    nameof(Address.Id),
                    nameof(Address.Name)
                ),
            };
            
            return View(vm);
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Inventory.EndedAt.HasValue)
                {
                    vm.Inventory.EndedAt = DateTime.SpecifyKind(vm.Inventory.EndedAt.Value, DateTimeKind.Utc);
                }
                
                if (!string.IsNullOrWhiteSpace(vm.RolesInput))
                {
                    vm.Inventory.AllowedRoles = vm.RolesInput
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .ToList();
                }
                
                _bll.InventoryService.Add(vm.Inventory);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                nameof(Address.Id), nameof(Address.Name), vm.Inventory.AddressId);

            return View(vm);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _bll.InventoryService.FindAsync(id.Value, User.GetUserId());
            if (inventory == null)
            {
                return NotFound();
            }
            
            var vm = new InventoryCreateEditViewModel
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                    nameof(Address.Id),
                    nameof(Address.Name),
                    inventory.AddressId
                ),
                
                Inventory = inventory,
                
                RolesInput = inventory.AllowedRoles != null ? string.Join(",", inventory.AllowedRoles) : ""
            };
            return View(vm);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, InventoryCreateEditViewModel vm)
        {
            if (id != vm.Inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (vm.Inventory.EndedAt.HasValue)
                {
                    vm.Inventory.EndedAt = DateTime.SpecifyKind(vm.Inventory.EndedAt.Value, DateTimeKind.Utc);
                }
                
                if (!string.IsNullOrWhiteSpace(vm.RolesInput))
                {
                    vm.Inventory.AllowedRoles = vm.RolesInput
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .ToList();
                }
                else
                {
                    vm.Inventory.AllowedRoles = new List<string>();
                }
                
                _bll.InventoryService.Update(vm.Inventory);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                nameof(Address.Id), nameof(Address.Name), vm.Inventory.AddressId);

            return View(vm);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var action = await _bll.InventoryService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                return NotFound();
            }

            return View(action);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.InventoryService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
