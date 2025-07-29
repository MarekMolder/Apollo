using App.DAL.EF;
using App.DTO.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MonthlyStatistics = App.Domain.Logic.MonthlyStatistics;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class MonthlyStatisticsController : Controller
    {
        private readonly AppDbContext _context;

        public MonthlyStatisticsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CurrentStocks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.MonthlyStatistics.Include(c => c.Product).Include(c => c.StorageRoom);
            return View(await appDbContext.ToListAsync());
        }

        // GET: CurrentStocks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyStatistics = await _context.MonthlyStatistics
                .Include(c => c.Product)
                .Include(c => c.StorageRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monthlyStatistics == null)
            {
                return NotFound();
            }

            return View(monthlyStatistics);
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
        public async Task<IActionResult> Create([Bind("TotalRemovedQuantity,ProductId,StorageRoomId,Year,Month,PeriodStart,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] App.Domain.Logic.MonthlyStatistics monthlyStatistics)
        {
            if (ModelState.IsValid)
            {
                monthlyStatistics.Id = Guid.NewGuid();
                _context.Add(monthlyStatistics);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", monthlyStatistics.ProductId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", monthlyStatistics.StorageRoomId);
            return View(monthlyStatistics);
        }

        // GET: CurrentStocks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyStatistics = await _context.MonthlyStatistics.FindAsync(id);
            if (monthlyStatistics == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", monthlyStatistics.ProductId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", monthlyStatistics.StorageRoomId);
            return View(monthlyStatistics);
        }

        // POST: CurrentStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TotalRemovedQuantity,ProductId,StorageRoomId,Year,Month,PeriodStart,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] MonthlyStatistics monthlyStatistics)
        {
            if (id != monthlyStatistics.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monthlyStatistics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonthlyStatisticsExists(monthlyStatistics.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", monthlyStatistics.ProductId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", monthlyStatistics.StorageRoomId);
            return View(monthlyStatistics);
        }

        // GET: CurrentStocks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyStatistics = await _context.MonthlyStatistics
                .Include(c => c.Product)
                .Include(c => c.StorageRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monthlyStatistics == null)
            {
                return NotFound();
            }

            return View(monthlyStatistics);
        }

        // POST: CurrentStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var monthlyStatistics = await _context.MonthlyStatistics.FindAsync(id);
            if (monthlyStatistics != null)
            {
                _context.MonthlyStatistics.Remove(monthlyStatistics);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonthlyStatisticsExists(Guid id)
        {
            return _context.MonthlyStatistics.Any(e => e.Id == id);
        }
    }
}
