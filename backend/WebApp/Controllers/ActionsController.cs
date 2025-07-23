using App.BLL.Contracts;
using App.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.DAL.DTO;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels;
using IAppBLL = App.BLL.Contracts.IAppBLL;

namespace WebApp.Controllers
{
    [Authorize]
    public class ActionsController : Controller
    {
        private readonly IAppBLL _bll;
        
        public ActionsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Actions
        public async Task<IActionResult> Index()
        {
            var res = await _bll.ActionEntityService.AllAsync(User.GetUserId());
            return View(res);

        }

        // GET: Actions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.ActionEntityService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: Actions/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ActionEntityCreateEditViewModel()
            {
                ActionTypeSelectList = new SelectList(await _bll.ActionTypeEntityService.AllAsync(User.GetUserId()),
                    nameof(ActionTypeEntity.Id),
                    nameof(ActionTypeEntity.Name)
                ),

                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name)
                ),

                ReasonSelectList = new SelectList(await _bll.ReasonService.AllAsync(User.GetUserId()),
                    nameof(Reason.Id),
                    nameof(Reason.Description)
                ),

                StockAuditSelectList = new SelectList(await _bll.StockAuditService.AllAsync(User.GetUserId()),
                    nameof(StockAudit.Id),
                    nameof(StockAudit.Id)
                ),

                SupplierSelectList = new SelectList(await _bll.SupplierService.AllAsync(User.GetUserId()),
                    nameof(Supplier.Id),
                    nameof(Supplier.Name)
                ),
                
                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id),
                nameof(StorageRoom.Name)
                ),
            };
            
            return View(vm);
        }

        // POST: Actions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActionEntityCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.ActionEntityService.Add(vm.ActionEntity, User.GetUserId());
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.ActionTypeSelectList = new SelectList(await _bll.ActionTypeEntityService.AllAsync(User.GetUserId()),
                nameof(ActionTypeEntity.Id), nameof(ActionTypeEntity.Name), vm.ActionEntity.ActionTypeId);
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.ActionEntity.ProductId);
            vm.ReasonSelectList = new SelectList(await _bll.ReasonService.AllAsync(User.GetUserId()),
                nameof(Reason.Id), nameof(Reason.Description), vm.ActionEntity.ReasonId);
            vm.StockAuditSelectList = new SelectList(await _bll.StockAuditService.AllAsync(User.GetUserId()),
                nameof(StockAudit.Id),nameof(StockAudit.Id), vm.ActionEntity.StockAuditId);
            vm.SupplierSelectList = new SelectList(await _bll.SupplierService.AllAsync(User.GetUserId()),
                nameof(Supplier.Id), nameof(Supplier.Name), vm.ActionEntity.SupplierId);
            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.ActionEntity.StorageRoomId);

            return View(vm);
        }

        // GET: Actions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actionEntity = await _bll.ActionEntityService.FindAsync(id.Value, User.GetUserId());
            if (actionEntity == null)
            {
                return NotFound();
            }
            
            var vm = new ActionEntityCreateEditViewModel()
            {
                ActionTypeSelectList = new SelectList(await _bll.ActionTypeEntityService.AllAsync(User.GetUserId()),
                    nameof(ActionTypeEntity.Id),
                    nameof(ActionTypeEntity.Name),
                    actionEntity.ActionTypeId
                ),

                ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                    nameof(Product.Id),
                    nameof(Product.Name),
                    actionEntity.ProductId
                ),

                ReasonSelectList = new SelectList(await _bll.ReasonService.AllAsync(User.GetUserId()),
                    nameof(Reason.Id),
                    nameof(Reason.Description),
                    actionEntity.ReasonId
                ),

                StockAuditSelectList = new SelectList(await _bll.StockAuditService.AllAsync(User.GetUserId()),
                    nameof(StockAudit.Id),
                    nameof(StockAudit.Id),
                    actionEntity.StockAuditId
                ),

                SupplierSelectList = new SelectList(await _bll.SupplierService.AllAsync(User.GetUserId()),
                    nameof(Supplier.Id),
                    nameof(Supplier.Name),
                    actionEntity.SupplierId
                ),
                StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                    nameof(StorageRoom.Id),
                    nameof(StorageRoom.Name),
                    actionEntity.StorageRoomId
                ),
                ActionEntity = actionEntity
            };
            return View(vm);
        }

        // POST: Actions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ActionEntityCreateEditViewModel vm)
        {
            if (id != vm.ActionEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // TODO: Chati lahendus proovi järgi
                //vm.ActionEntity.UserId = User.GetUserId();
                
                _bll.ActionEntityService.Update(vm.ActionEntity);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.ActionTypeSelectList = new SelectList(await _bll.ActionTypeEntityService.AllAsync(User.GetUserId()),
                nameof(ActionTypeEntity.Id), nameof(ActionTypeEntity.Name), vm.ActionEntity.ActionTypeId);
            vm.ProductSelectList = new SelectList(await _bll.ProductService.AllAsync(User.GetUserId()),
                nameof(Product.Id), nameof(Product.Name), vm.ActionEntity.ProductId);
            vm.ReasonSelectList = new SelectList(await _bll.ReasonService.AllAsync(User.GetUserId()),
                nameof(Reason.Id), nameof(Reason.Description), vm.ActionEntity.ReasonId);
            vm.StockAuditSelectList = new SelectList(await _bll.StockAuditService.AllAsync(User.GetUserId()),
                nameof(StockAudit.Id),nameof(StockAudit.Id), vm.ActionEntity.StockAuditId);
            vm.SupplierSelectList = new SelectList(await _bll.SupplierService.AllAsync(User.GetUserId()),
                nameof(Supplier.Id), nameof(Supplier.Name), vm.ActionEntity.SupplierId);
            vm.StorageRoomSelectList = new SelectList(await _bll.StorageRoomService.AllAsync(User.GetUserId()),
                nameof(StorageRoom.Id), nameof(StorageRoom.Name), vm.ActionEntity.StorageRoomId);

            return View(vm);
        }

        // GET: Actions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var action = await _bll.ActionEntityService.FindAsync(id.Value, User.GetUserId());

            if (action == null)
            {
                return NotFound();
            }

            return View(action);
        }

        // POST: Actions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.ActionEntityService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
