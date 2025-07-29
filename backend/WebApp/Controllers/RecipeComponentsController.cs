using App.BLL.Contracts;
using App.DAL.EF;
using App.DTO.v1;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;

namespace WebApp.Controllers{
    [Authorize(Roles = "admin")]
    public class RecipeComponentsController : Controller
    {
        private readonly IAppBLL _bll;

        public RecipeComponentsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: CurrentStocks
        public async Task<IActionResult> Index()
        {
            var res = await _bll.RecipeComponentService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: CurrentStocks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.RecipeComponentService.FindAsync(id.Value,User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Actions/Create
        public async Task<IActionResult> Create()
        {
            var vm = new RecipeComponentsCreateEditViewModel()
            {
                ProductRecipeSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),

                ComponentProductSelectList = new SelectList(
                    (await _bll.ProductService.AllAsync(User.GetUserId()))
                    .Where(p => p.IsComponent),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),
            };
            
            return View(vm);
        }

        // POST: Actions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeComponentsCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.RecipeComponentService.Add(vm.RecipeComponent, User.GetUserId());
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.ProductRecipeSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.RecipeComponent.ProductRecipeId);
            vm.ComponentProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.RecipeComponent.ComponentProductId);
            

            return View(vm);
        }

        // GET: Actions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeComponent = await _bll.RecipeComponentService.FindAsync(id.Value, User.GetUserId());
            if (recipeComponent == null)
            {
                return NotFound();
            }
            
            var allProducts = await _bll.ProductService.AllAsync(User.GetUserId());
            
            var vm = new RecipeComponentsCreateEditViewModel()
            {
                ProductRecipeSelectList = new SelectList(
                    allProducts,
                    nameof(Product.Id),
                    nameof(Product.Name),
                    recipeComponent.ProductRecipeId
                ),
                
                ComponentProductSelectList = new SelectList(
                    allProducts.Where(p => p.IsComponent),
                    nameof(Product.Id),
                    nameof(Product.Name),
                    recipeComponent.ComponentProductId
                ),
                
                RecipeComponent = recipeComponent
            };
            return View(vm);
        }

        // POST: Actions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RecipeComponentsCreateEditViewModel vm)
        {
            if (id != vm.RecipeComponent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.RecipeComponentService.Update(vm.RecipeComponent);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.ProductRecipeSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.RecipeComponent.ProductRecipeId);
            vm.ComponentProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.RecipeComponent.ComponentProductId);

            return View(vm);
        }

        // GET: Actions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeComponent = await _bll.RecipeComponentService.FindAsync(id.Value, User.GetUserId());

            if (recipeComponent == null)
            {
                return NotFound();
            }

            return View(recipeComponent);
        }

        // POST: Actions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.RecipeComponentService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}