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
    public class StorageRoomInInventoriesController : Controller
    {
        private readonly AppDbContext _context;

        public StorageRoomInInventoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StorageRoomInInventories
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.StorageRoomInInventories.Include(s => s.Inventory).Include(s => s.StorageRoom);
            return View(await appDbContext.ToListAsync());
        }

        // GET: StorageRoomInInventories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageRoomInInventory = await _context.StorageRoomInInventories
                .Include(s => s.Inventory)
                .Include(s => s.StorageRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storageRoomInInventory == null)
            {
                return NotFound();
            }

            return View(storageRoomInInventory);
        }

        // GET: StorageRoomInInventories/Create
        public IActionResult Create()
        {
            ViewData["InventoryId"] = new SelectList(_context.Inventories, "Id", "CreatedBy");
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy");
            return View();
        }

        // POST: StorageRoomInInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EndedAt,InventoryId,StorageRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] StorageRoomInInventory storageRoomInInventory)
        {
            if (ModelState.IsValid)
            {
                storageRoomInInventory.Id = Guid.NewGuid();
                _context.Add(storageRoomInInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InventoryId"] = new SelectList(_context.Inventories, "Id", "CreatedBy", storageRoomInInventory.InventoryId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", storageRoomInInventory.StorageRoomId);
            return View(storageRoomInInventory);
        }

        // GET: StorageRoomInInventories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageRoomInInventory = await _context.StorageRoomInInventories.FindAsync(id);
            if (storageRoomInInventory == null)
            {
                return NotFound();
            }
            ViewData["InventoryId"] = new SelectList(_context.Inventories, "Id", "CreatedBy", storageRoomInInventory.InventoryId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", storageRoomInInventory.StorageRoomId);
            return View(storageRoomInInventory);
        }

        // POST: StorageRoomInInventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("EndedAt,InventoryId,StorageRoomId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] StorageRoomInInventory storageRoomInInventory)
        {
            if (id != storageRoomInInventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storageRoomInInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StorageRoomInInventoryExists(storageRoomInInventory.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["InventoryId"] = new SelectList(_context.Inventories, "Id", "CreatedBy", storageRoomInInventory.InventoryId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", storageRoomInInventory.StorageRoomId);
            return View(storageRoomInInventory);
        }

        // GET: StorageRoomInInventories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageRoomInInventory = await _context.StorageRoomInInventories
                .Include(s => s.Inventory)
                .Include(s => s.StorageRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storageRoomInInventory == null)
            {
                return NotFound();
            }

            return View(storageRoomInInventory);
        }

        // POST: StorageRoomInInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var storageRoomInInventory = await _context.StorageRoomInInventories.FindAsync(id);
            if (storageRoomInInventory != null)
            {
                _context.StorageRoomInInventories.Remove(storageRoomInInventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorageRoomInInventoryExists(Guid id)
        {
            return _context.StorageRoomInInventories.Any(e => e.Id == id);
        }
    }
}
