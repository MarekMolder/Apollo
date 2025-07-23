using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager")]
    public class StockAuditsController : ControllerBase
    {
        private readonly ILogger<Controllers.StockAuditsController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly App.DTO.v1.Mappers.StockAuditAPIMapper _mapper =
            new App.DTO.v1.Mappers.StockAuditAPIMapper();

        public StockAuditsController(IAppBLL bll, ILogger<Controllers.StockAuditsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all persons for current user
        /// </summary>
        /// <returns>List of persons</returns>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<App.DTO.v1.StockAudit> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.StockAudit>>> GetActions()
        {
            return (await _bll.StockAuditService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.StockAudit>> GetActionEntity(Guid id)
        {
            var stockAudit = await _bll.StockAuditService.FindAsync(id);

            if (stockAudit == null)
            {
                return NotFound();
            }

            return _mapper.Map(stockAudit)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, App.DTO.v1.StockAudit stockAudit)
        {
            if (id != stockAudit.Id)
            {
                return BadRequest();
            }

            await _bll.StockAuditService.UpdateAsync(_mapper.Map(stockAudit)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.StockAudit>> PostActionEntity(App.DTO.v1.StockAudit stockAudit)
        {
            var bllEntity = _mapper.Map(stockAudit);
            _bll.StockAuditService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetStockAudit", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, stockAudit);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.StockAuditService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
