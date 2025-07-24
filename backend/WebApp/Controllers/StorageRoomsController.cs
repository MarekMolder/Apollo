using App.BLL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAppBLL = App.BLL.Contracts.IAppBLL;

namespace WebApp.Controllers
{
    [Authorize]
    public class StorageRoomsController : Controller
    {
        private readonly IAppBLL _bll;

        public StorageRoomsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: StorageRooms
        public async Task<IActionResult> Index()
        {
            var res = await _bll.StorageRoomService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: StorageRooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: StorageRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StorageRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StorageRoom storageRoom)
        {
            if (ModelState.IsValid)
            {
                if (storageRoom.EndedAt.HasValue)
                { 
                    storageRoom.EndedAt = DateTime.SpecifyKind(storageRoom.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.StorageRoomService.Add(storageRoom);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(storageRoom);
        }

        // GET: StorageRooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: StorageRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StorageRoom storageRoom)
        {
            if (id != storageRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.StorageRoomService.Update(storageRoom);
                await _bll.SaveChangesAsync();
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


            var entity = await _bll.StorageRoomService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: StorageRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.StorageRoomService.RemoveAsync(id, User.GetUserId());

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
