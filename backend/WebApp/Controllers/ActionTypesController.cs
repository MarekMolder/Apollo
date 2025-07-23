using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Contracts;
using App.DAL.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Logic;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using IAppBLL = App.BLL.Contracts.IAppBLL;

namespace WebApp.Controllers
{
    [Authorize]
    public class ActionTypesController : Controller
    {
        private readonly IAppBLL _bll;
        
        public ActionTypesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: ActionTypes
        public async Task<IActionResult> Index()
        {
            var res = await _bll.ActionTypeEntityService.AllAsync(User.GetUserId());
            return View(res);
        }

        // GET: ActionTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = await _bll.ActionTypeEntityService.FindAsync(id.Value, User.GetUserId());
            
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // GET: ActionTypes/Create
        public IActionResult Create()
        {
            PopulateEnumSelectList();
            return View();
        }

        // POST: ActionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(App.BLL.DTO.ActionTypeEntity actionTypeEntity)
        {
            if (ModelState.IsValid)
            {
                if (actionTypeEntity.EndedAt.HasValue)
                {
                    actionTypeEntity.EndedAt = DateTime.SpecifyKind(actionTypeEntity.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.ActionTypeEntityService.Add(actionTypeEntity);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateEnumSelectList();
            return View(actionTypeEntity);
        }

        // GET: ActionTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.ActionTypeEntityService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            PopulateEnumSelectList();
            return View(entity);
        }

        // POST: ActionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, App.BLL.DTO.ActionTypeEntity actionTypeEntity)
        {
            if (id != actionTypeEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                if (actionTypeEntity.EndedAt.HasValue)
                {
                    actionTypeEntity.EndedAt = DateTime.SpecifyKind(actionTypeEntity.EndedAt.Value, DateTimeKind.Utc);
                }
                
                _bll.ActionTypeEntityService.Update(actionTypeEntity);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            PopulateEnumSelectList();
            return View(actionTypeEntity);
        }

        // GET: ActionTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var entity = await _bll.ActionTypeEntityService.FindAsync(id.Value, User.GetUserId());
            if (entity == null)
            {
                return NotFound();
            }

            return View(entity);
        }

        // POST: ActionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.ActionTypeEntityService.RemoveAsync(id, User.GetUserId());

            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private void PopulateEnumSelectList()
        {
            ViewData["ActionTypeEnumList"] = new SelectList(
                Enum.GetValues(typeof(App.Domain.Enums.ActionTypeEnum))
                    .Cast<App.Domain.Enums.ActionTypeEnum>()
                    .Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.ToString()
                    }),
                "Value",
                "Text"
            );
        }
        
    }
}
