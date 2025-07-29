using App.DAL.EF;
using App.DTO.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class RecipeComponentController : Controller
    {
        private readonly AppDbContext _context;

        public RecipeComponentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CurrentStocks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.RecipeComponents.Include(c => c.ProductRecipe).Include(c => c.ComponentProduct);
            return View(await appDbContext.ToListAsync());
        }

        // GET: CurrentStocks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeComponent = await _context.RecipeComponents
                .Include(c => c.ProductRecipe)
                .Include(c => c.ComponentProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeComponent == null)
            {
                return NotFound();
            }

            return View(recipeComponent);
        }

        // GET: CurrentStocks/Create
        public IActionResult Create()
        {
            ViewData["ProductRecipeId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["ComponentProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: CurrentStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductRecipeId,ComponentProductId,Amount,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] RecipeComponent recipeComponent)
        {
            if (ModelState.IsValid)
            {
                recipeComponent.Id = Guid.NewGuid();
                _context.Add(recipeComponent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductRecipeId"] = new SelectList(_context.Products, "Id", "Name", recipeComponent.ProductRecipeId);
            ViewData["ComponentProductId"] = new SelectList(_context.Products, "Id", "Name", recipeComponent.ComponentProductId);
            return View(recipeComponent);
        }

        // GET: CurrentStocks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeComponent = await _context.RecipeComponents.FindAsync(id);
            if (recipeComponent == null)
            {
                return NotFound();
            }
            ViewData["ProductRecipeId"] = new SelectList(_context.Products, "Id", "Name", recipeComponent.ProductRecipeId);
            ViewData["ComponentProductId"] = new SelectList(_context.Products, "Id", "Name", recipeComponent.ComponentProductId);
            return View(recipeComponent);
        }

        // POST: CurrentStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductRecipeId,ComponentProductId,AmountPerUnit,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] RecipeComponent recipeComponent)
        {
            if (id != recipeComponent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeComponent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeComponentExists(recipeComponent.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductRecipeId"] = new SelectList(_context.Products, "Id", "Name", recipeComponent.ProductRecipeId);
            ViewData["ComponentProductId"] = new SelectList(_context.Products, "Id", "Name", recipeComponent.ComponentProductId);
            return View(recipeComponent);
        }

        // GET: CurrentStocks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeComponent = await _context.RecipeComponents
                .Include(c => c.ProductRecipe)
                .Include(c => c.ComponentProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeComponent == null)
            {
                return NotFound();
            }

            return View(recipeComponent);
        }

        // POST: CurrentStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recipeComponent = await _context.RecipeComponents.FindAsync(id);
            if (recipeComponent != null)
            {
                _context.RecipeComponents.Remove(recipeComponent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeComponentExists(Guid id)
        {
            return _context.RecipeComponents.Any(e => e.Id == id);
        }
    }
}