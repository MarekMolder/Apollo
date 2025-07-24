using App.DAL.EF;
using App.Domain.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas_Admin_Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class ActionTypesController : Controller
    {
        private readonly AppDbContext _context;

        public ActionTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ActionTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActionTypes.ToListAsync());
        }

        // GET: ActionTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionTypeEntity = await _context.ActionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionTypeEntity == null)
            {
                return NotFound();
            }

            return View(actionTypeEntity);
        }

        // GET: ActionTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,EndedAt,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] ActionTypeEntity actionTypeEntity)
        {
            if (ModelState.IsValid)
            {
                actionTypeEntity.Id = Guid.NewGuid();
                _context.Add(actionTypeEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actionTypeEntity);
        }

        // GET: ActionTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionTypeEntity = await _context.ActionTypes.FindAsync(id);
            if (actionTypeEntity == null)
            {
                return NotFound();
            }
            return View(actionTypeEntity);
        }

        // POST: ActionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,EndedAt,Id,CreatedBy,CreatedAt,ChangedBy,ChangedAt,SysNotes")] ActionTypeEntity actionTypeEntity)
        {
            if (id != actionTypeEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actionTypeEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActionTypeEntityExists(actionTypeEntity.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(actionTypeEntity);
        }

        // GET: ActionTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionTypeEntity = await _context.ActionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionTypeEntity == null)
            {
                return NotFound();
            }

            return View(actionTypeEntity);
        }

        // POST: ActionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var actionTypeEntity = await _context.ActionTypes.FindAsync(id);
            if (actionTypeEntity != null)
            {
                _context.ActionTypes.Remove(actionTypeEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActionTypeEntityExists(Guid id)
        {
            return _context.ActionTypes.Any(e => e.Id == id);
        }
    }
}
