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
    public class ProductsController : Controller
    {
        private readonly IAppBLL _bll;

        public ProductsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var res = await _bll.ProductService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.ProductService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ProductCreateEditViewModel()
            {
                ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                    nameof(ProductCategory.Id),
                    nameof(ProductCategory.Name)
                ),
            };
            
            return View(vm);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.ProductService.Add(vm.Product);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                nameof(ProductCategory.Id), nameof(ProductCategory.Name), vm.Product.ProductCategoryId);

            return View(vm);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _bll.ProductService.FindAsync(id.Value, User.GetUserId());
            if (product == null)
            {
                return NotFound();
            }
            
            var vm = new ProductCreateEditViewModel()
            {
                ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                    nameof(ProductCategory.Id),
                    nameof(ProductCategory.Name),
                    product.ProductCategoryId
                ),
                Product = product
            };
            return View(vm);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductCreateEditViewModel vm)
        {
            if (id != vm.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.ProductService.Update(vm.Product);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.ProductCategorySelectList = new SelectList(await _bll.ProductCategoryService.AllAsync(User.GetUserId()),
                nameof(ProductCategory.Id), nameof(ProductCategory.Name), vm.Product.ProductCategory);

            return View(vm);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var action = await _bll.ProductService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                return NotFound();
            }

            return View(action);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.ProductService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
