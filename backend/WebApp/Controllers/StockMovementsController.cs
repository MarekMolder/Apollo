using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels;
using IAppBLL = App.BLL.Contracts.IAppBLL;

namespace WebApp.Controllers
{
    [Authorize]
    public class StockMovementsController : Controller
    {
        private readonly IAppBLL _bll;

        public StockMovementsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: StockMovements
        public async Task<IActionResult> Index()
        {
            var res = await _bll.StockMovementService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: StockMovements/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.StockMovementService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: StockMovements/Create
        public async Task<IActionResult> Create()
        {
            var vm = new StockMovementCreateEditViewModel()
            {
                FromInventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                    nameof(Inventory.Id),
                    nameof(Inventory.Name)
                ),

                FromStorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name)
                ),
                
                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),

                ToInventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                    nameof(Inventory.Id),
                    nameof(Inventory.Id)
                ),

                ToStorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name)
                )
            };
            
            return View(vm);
            
            /*
            ViewData["FromInventoryId"] = new SelectList(_context.Inventories, "Id", "Name");
            ViewData["FromStorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "Location");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code");
            ViewData["ToInventoryId"] = new SelectList(_context.Inventories, "Id", "Name");
            ViewData["ToStorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "Location");
            return View();
            */
        }

        // POST: StockMovements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockMovementCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.StockMovementService.Add(vm.StockMovement);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.FromInventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                nameof(Inventory.Id), nameof(Inventory.Name), vm.StockMovement.FromInventoryId);
            vm.FromStorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.StockMovement.FromStorageRoomId);
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.StockMovement.ProductId);
            vm.ToInventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                nameof(Inventory.Id),nameof(Inventory.Id), vm.StockMovement.ToInventoryId);
            vm.ToStorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.StockMovement.ToStorageRoomId);

            return View(vm);
        }

        // GET: StockMovements/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockMovement = await _bll.StockMovementService.FindAsync(id.Value, User.GetUserId());
            if (stockMovement == null)
            {
                return NotFound();
            }
            
            var vm = new StockMovementCreateEditViewModel()
            {
                FromInventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                    nameof(Inventory.Id),
                    nameof(Inventory.Name),
                    stockMovement.FromInventoryId
                ),
                
                FromStorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    stockMovement.FromStorageRoomId
                ),

                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name),
                    stockMovement.ProductId
                ),
                
                ToInventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                    nameof(Inventory.Id),
                    nameof(Inventory.Id),
                    stockMovement.ToInventoryId
                ),

                ToStorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    stockMovement.ToStorageRoomId
                ),
                StockMovement = stockMovement
            };
            return View(vm);
        }

        // POST: StockMovements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StockMovementCreateEditViewModel vm)
        {
            if (id != vm.StockMovement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.StockMovementService.Update(vm.StockMovement);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.FromInventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                nameof(Inventory.Id), nameof(Inventory.Name), vm.StockMovement.FromInventoryId);
            vm.FromStorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.StockMovement.FromStorageRoomId);
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.StockMovement.ProductId);
            vm.ToInventorySelectList = new SelectList(await _bll.InventoryService.AllAsync(User.GetUserId()),
                nameof(Inventory.Id),nameof(Inventory.Id), vm.StockMovement.ToInventoryId);
            vm.ToStorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.StockMovement.ToStorageRoomId);

            return View(vm);
        }

        // GET: StockMovements/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var action = await _bll.StockMovementService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                return NotFound();
            }

            return View(action);
        }

        // POST: StockMovements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.StockMovementService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
