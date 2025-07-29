using App.BLL.Contracts;
using App.DAL.EF;
using App.DTO.v1;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;
using MonthlyStatistics = App.Domain.Logic.MonthlyStatistics;

namespace WebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class MonthlyStatisticsController : Controller
    {
        private readonly IAppBLL _bll;

        public MonthlyStatisticsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: CurrentStocks
        public async Task<IActionResult> Index()
        {
            var res = await _bll.MonthlyStatisticsService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: CurrentStocks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.MonthlyStatisticsService.FindAsync(id.Value,User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: CurrentStocks/Create
        public async Task<IActionResult> Create()
        {
            var vm = new MonthlyStatisticsCreateEditViewModel()
            {
                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),

                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name)
                ),
            };
            
            return View(vm);
        }

        // POST: CurrentStocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonthlyStatisticsCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.MonthlyStatisticsService.Add(vm.MonthlyStatistics, User.GetUserId());
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.MonthlyStatistics.ProductId);
            vm.StorageRoomSelectList = new SelectList(await _bll.MonthlyStatisticsService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.MonthlyStatistics.StorageRoomId);
            

            return View(vm);
        }

        // GET: CurrentStocks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyStatistics = await _bll.MonthlyStatisticsService.FindAsync(id.Value, User.GetUserId());
            if (monthlyStatistics == null)
            {
                return NotFound();
            }
            
            var vm = new MonthlyStatisticsCreateEditViewModel()
            {
                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name),
                    monthlyStatistics.ProductId
                ),
                
                StorageRoomSelectList = new SelectList(await _bll.MonthlyStatisticsService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    monthlyStatistics.StorageRoomId
                ),
                
                MonthlyStatistics = monthlyStatistics
            };
            return View(vm);
        }

        // POST: CurrentStocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MonthlyStatisticsCreateEditViewModel vm)
        {
            if (id != vm.MonthlyStatistics.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _bll.MonthlyStatisticsService.Update(vm.MonthlyStatistics);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.MonthlyStatistics.ProductId);
            vm.StorageRoomSelectList = new SelectList(await _bll.RecipeComponentService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.MonthlyStatistics.StorageRoomId);

            return View(vm);
        }

        // GET: CurrentStocks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monthlyStatistics = await _bll.MonthlyStatisticsService.FindAsync(id.Value, User.GetUserId());

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
            await _bll.MonthlyStatisticsService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
