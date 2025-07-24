using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAppBLL = App.BLL.Contracts.IAppBLL;

namespace WebApp.Controllers
{
    [Authorize]
    public class ReasonsController : Controller
    {
        private readonly IAppBLL _bll;

        public ReasonsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Reasons
        public async Task<IActionResult> Index()
        {
            var res = await _bll.ReasonService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: Reasons/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.ReasonService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Reasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reason reason)
        {
            if (ModelState.IsValid)
            {
                if (reason.EndedAt.HasValue)
                { 
                    reason.EndedAt = DateTime.SpecifyKind(reason.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.ReasonService.Add(reason);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(reason);
        }

        // GET: Reasons/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.ReasonService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Reasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Reason reason)
        {
            if (id != reason.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (reason.EndedAt.HasValue)
                { 
                    reason.EndedAt = DateTime.SpecifyKind(reason.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.ReasonService.Update(reason);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(reason);
        }

        // GET: Reasons/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.ReasonService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Reasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.ReasonService.RemoveAsync(id, User.GetUserId());

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
