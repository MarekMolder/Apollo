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
using App.DAL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels;
using IAppBLL = App.BLL.Contracts.IAppBLL;

namespace WebApp.Controllers
{
    [Authorize]
    public class StockAuditsController : Controller
    {
        private readonly IAppBLL _bll;

        public StockAuditsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: StockAudits
        public async Task<IActionResult> Index()
        {
            var res = await _bll.StockAuditService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: StockAudits/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.StockAuditService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: StockAudits/Create
        public async Task<IActionResult> Create()
        {
            var vm = new StockAuditCreateViewModel()
            {
                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name)
                ),
            };
            
            return View(vm);
            
        }

        // POST: StockAudits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockAuditCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.StockAuditService.Add(vm.StockAudit);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.StockAudit.StorageRoomId);

            return View(vm);
        }

        // GET: StockAudits/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockAudit = await _bll.StockAuditService.FindAsync(id.Value, User.GetUserId());
            if (stockAudit == null)
            {
                return NotFound();
            }
            
            var vm = new StockAuditCreateViewModel()
            {
                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    stockAudit.StorageRoomId
                ),
                
                StockAudit = stockAudit
            };
            return View(vm);
        }

        // POST: StockAudits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StockAuditCreateViewModel vm)
        {
            if (id != vm.StockAudit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.StockAuditService.Update(vm.StockAudit);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.StockAudit.StorageRoomId);

            return View(vm);
        }

        // GET: StockAudits/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var action = await _bll.StockAuditService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                return NotFound();
            }

            return View(action);
        }

        // POST: StockAudits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.StockAuditService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
