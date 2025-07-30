using App.BLL.Contracts;
using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class AddressesController : Controller
    {
        private readonly IAppBll _bll;

        public AddressesController(IAppBll bll)
        {
            _bll = bll;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var res = await _bll.AddressService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.AddressService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if (ModelState.IsValid)
            {
                _bll.AddressService.Add(address);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.AddressService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.AddressService.Update(address);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.AddressService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.AddressService.RemoveAsync(id, User.GetUserId());

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
