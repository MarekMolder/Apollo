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
    public class SuppliersController : Controller
    {
        private readonly IAppBLL _bll;

        public SuppliersController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            var res = await _bll.SupplierService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.SupplierService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Suppliers/Create
        public async Task<IActionResult> Create()
        {
            var vm = new SupplierCreateEditViewModel()
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                    nameof(Address.Id),
                    nameof(Address.Name)
                )
            };
            
            return View(vm);
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.SupplierService.Add(vm.Supplier);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                nameof(Address.Id), nameof(Address.Name), vm.Supplier.AddressId);
            
            return View(vm);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _bll.SupplierService.FindAsync(id.Value, User.GetUserId());
            if (supplier == null)
            {
                return NotFound();
            }
            
            var vm = new SupplierCreateEditViewModel()
            {
                AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                    nameof(Address.Id),
                    nameof(Address.Name),
                    supplier.AddressId
                ),
                Supplier = supplier
            };
            return View(vm);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SupplierCreateEditViewModel vm)
        {
            if (id != vm.Supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.SupplierService.Update(vm.Supplier);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.AddressSelectList = new SelectList(await _bll.AddressService.AllAsync(User.GetUserId()),
                nameof(Address.Id), nameof(Address.Name), vm.Supplier.AddressId);

            return View(vm);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _bll.SupplierService.FindAsync(id.Value, User.GetUserId());

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.SupplierService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
