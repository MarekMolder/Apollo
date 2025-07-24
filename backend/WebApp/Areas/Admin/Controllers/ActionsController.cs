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
    public class ActionsController : Controller
    {
        private readonly AppDbContext _context;

        public ActionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Actions
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Actions.Include(a => a.ActionType).Include(a => a.Product).Include(a => a.Reason).Include(a => a.Supplier).Include(a => a.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Actions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionEntity = await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.Product)
                .Include(a => a.Reason)
                .Include(c => c.StorageRoom)
                .Include(a => a.Supplier)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionEntity == null)
            {
                return NotFound();
            }

            return View(actionEntity);
        }

        // GET: Actions/Create
        public IActionResult Create()
        {
            ViewData["ActionTypeId"] = new SelectList(_context.ActionTypes, "Id", "CreatedBy");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code");
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "Id", "CreatedBy");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CreatedBy");
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: Actions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Quantity,Status,ActionTypeId,ReasonId,SupplierId,ProductId,StockAuditId,UserId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] ActionEntity actionEntity)
        {
            if (ModelState.IsValid)
            {
                actionEntity.Id = Guid.NewGuid();
                _context.Add(actionEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActionTypeId"] = new SelectList(_context.ActionTypes, "Id", "CreatedBy", actionEntity.ActionTypeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", actionEntity.ProductId);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "Id", "CreatedBy", actionEntity.ReasonId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CreatedBy", actionEntity.SupplierId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", actionEntity.StorageRoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", actionEntity.UserId);
            return View(actionEntity);
        }

        // GET: Actions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionEntity = await _context.Actions.FindAsync(id);
            if (actionEntity == null)
            {
                return NotFound();
            }
            ViewData["ActionTypeId"] = new SelectList(_context.ActionTypes, "Id", "CreatedBy", actionEntity.ActionTypeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", actionEntity.ProductId);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "Id", "CreatedBy", actionEntity.ReasonId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CreatedBy", actionEntity.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", actionEntity.UserId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", actionEntity.StorageRoomId);
            return View(actionEntity);
        }

        // POST: Actions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Quantity,Status,ActionTypeId,ReasonId,SupplierId,ProductId,StockAuditId,UserId,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] ActionEntity actionEntity)
        {
            if (id != actionEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actionEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActionEntityExists(actionEntity.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActionTypeId"] = new SelectList(_context.ActionTypes, "Id", "CreatedBy", actionEntity.ActionTypeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Code", actionEntity.ProductId);
            ViewData["ReasonId"] = new SelectList(_context.Reasons, "Id", "CreatedBy", actionEntity.ReasonId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "CreatedBy", actionEntity.SupplierId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", actionEntity.UserId);
            ViewData["StorageRoomId"] = new SelectList(_context.StorageRooms, "Id", "CreatedBy", actionEntity.StorageRoomId);
            return View(actionEntity);
        }

        // GET: Actions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionEntity = await _context.Actions
                .Include(a => a.ActionType)
                .Include(a => a.Product)
                .Include(a => a.Reason)
                .Include(a => a.Supplier)
                .Include(a => a.User)
                .Include(c => c.StorageRoom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionEntity == null)
            {
                return NotFound();
            }

            return View(actionEntity);
        }

        // POST: Actions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var actionEntity = await _context.Actions.FindAsync(id);
            if (actionEntity != null)
            {
                _context.Actions.Remove(actionEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActionEntityExists(Guid id)
        {
            return _context.Actions.Any(e => e.Id == id);
        }
    }
}
