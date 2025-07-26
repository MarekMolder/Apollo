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
    public class StorageRoomsController : Controller
    {
        private readonly AppDbContext _context;

        public StorageRoomsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: StorageRooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.StorageRooms.ToListAsync());
        }

        // GET: StorageRooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageRoom = await _context.StorageRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storageRoom == null)
            {
                return NotFound();
            }

            return View(storageRoom);
        }

        // GET: StorageRooms/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City");
            return View();
        }

        // POST: StorageRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,EndedAt,AddressId,AllowedUsers,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] StorageRoom storageRoom)
        {
            if (ModelState.IsValid)
            {
                storageRoom.Id = Guid.NewGuid();
                _context.Add(storageRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City", storageRoom.AddressId);
            return View(storageRoom);
        }

        // GET: StorageRooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageRoom = await _context.StorageRooms.FindAsync(id);
            if (storageRoom == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City", storageRoom.AddressId);
            return View(storageRoom);
        }

        // POST: StorageRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,EndedAt,AddressId,AllowedUsers,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] StorageRoom storageRoom)
        {
            if (id != storageRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storageRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StorageRoomExists(storageRoom.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(storageRoom);
        }

        // GET: StorageRooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageRoom = await _context.StorageRooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (storageRoom == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City", storageRoom.AddressId);
            return View(storageRoom);
        }

        // POST: StorageRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var storageRoom = await _context.StorageRooms.FindAsync(id);
            if (storageRoom != null)
            {
                _context.StorageRooms.Remove(storageRoom);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorageRoomExists(Guid id)
        {
            return _context.StorageRooms.Any(e => e.Id == id);
        }
    }
}
