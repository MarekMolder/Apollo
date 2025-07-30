using App.BLL.Contracts;
using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProductCategoriesController : Controller
    {
        private readonly IAppBll _bll;

        public ProductCategoriesController(IAppBll bll)
        {
            _bll = bll;
        }

        // GET: ProductCategories
        public async Task<IActionResult> Index()
        {
            var res = await _bll.ProductCategoryService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: ProductCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.ProductCategoryService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: ProductCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                if (productCategory.EndedAt.HasValue)
                { 
                    productCategory.EndedAt = DateTime.SpecifyKind(productCategory.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.ProductCategoryService.Add(productCategory);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.ProductCategoryService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (productCategory.EndedAt.HasValue)
                { 
                    productCategory.EndedAt = DateTime.SpecifyKind(productCategory.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.ProductCategoryService.Update(productCategory);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.ProductCategoryService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.ProductCategoryService.RemoveAsync(id, User.GetUserId());

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
