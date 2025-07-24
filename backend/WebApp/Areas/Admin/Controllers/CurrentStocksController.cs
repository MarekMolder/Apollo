using App.DAL.EF;
using App.Domain.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas_Admin_Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class CurrentStocksController : Controller
    {
        private readonly AppDbContext _context;

        public CurrentStocksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CurrentStocks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.CurrentStocks.Include(c => c.Product).Include(c => c.StorageRoom);
            return View(await appDbContext.ToListAsync());
        }

        // GET: CurrentStocks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentStock = await _context.CurrentStocks
                .Include(c => c.Product)
                .Include(c => c.StorageRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currentStock == null)
            {
                return NotFound();
            }

            return View(currentStock);
        }

        // GET: CurrentStocks/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code");
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy");
            return View();
        }

        // POST: CurrentStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,ProductId,StorageRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] CurrentStock currentStock)
        {
            if (ModelState.IsValid)
            {
                currentStock.Id = Guid.NewGuid();
                _context.Add(currentStock);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", currentStock.ProductId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", currentStock.StorageRoomId);
            return View(currentStock);
        }

        // GET: CurrentStocks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentStock = await _context.CurrentStocks.FindAsync(id);
            if (currentStock == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", currentStock.ProductId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", currentStock.StorageRoomId);
            return View(currentStock);
        }

        // POST: CurrentStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Quantity,ProductId,StorageRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] CurrentStock currentStock)
        {
            if (id != currentStock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currentStock);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrentStockExists(currentStock.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", currentStock.ProductId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", currentStock.StorageRoomId);
            return View(currentStock);
        }

        // GET: CurrentStocks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentStock = await _context.CurrentStocks
                .Include(c => c.Product)
                .Include(c => c.StorageRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currentStock == null)
            {
                return NotFound();
            }

            return View(currentStock);
        }

        // POST: CurrentStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var currentStock = await _context.CurrentStocks.FindAsync(id);
            if (currentStock != null)
            {
                _context.CurrentStocks.Remove(currentStock);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrentStockExists(Guid id)
        {
            return _context.CurrentStocks.Any(e => e.Id == id);
        }
    }
}
