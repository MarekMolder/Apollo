using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Contracts;
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
    public class CurrentStocksController : Controller
    {
        private readonly IAppBLL _bll;

        public CurrentStocksController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: CurrentStocks
        public async Task<IActionResult> Index()
        {
            var res = await _bll.CurrentStockService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: CurrentStocks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.CurrentStockService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: CurrentStocks/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CurrentStockCreateEditViewModel()
            {
                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),
                
                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name)
                ),
                
            };
            
            return View(vm);
        }

        // POST: CurrentStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CurrentStockCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.CurrentStockService.Add(vm.CurrentStock);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.CurrentStock.ProductId);
            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.CurrentStock.StorageRoomId);
            
            return View(vm);
        }

        // GET: CurrentStocks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentStock = await _bll.CurrentStockService.FindAsync(id.Value, User.GetUserId());
            if (currentStock == null)
            {
                return NotFound();
            }
            
            var vm = new CurrentStockCreateEditViewModel()
            {
                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name),
                    currentStock.ProductId
                ),

                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    currentStock.StorageRoomId
                ),
                
                CurrentStock = currentStock
            };
            return View(vm);
        }

        // POST: CurrentStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CurrentStockCreateEditViewModel vm)
        {
            if (id != vm.CurrentStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.CurrentStockService.Update(vm.CurrentStock);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.CurrentStock.ProductId);
            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.CurrentStock.StorageRoomId);

            return View(vm);
        }

        // GET: CurrentStocks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var action = await _bll.CurrentStockService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                return NotFound();
            }

            return View(action);
        }

        // POST: CurrentStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.CurrentStockService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
