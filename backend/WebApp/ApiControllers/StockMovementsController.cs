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
    public class StockMovementsController : ControllerBase
    {
        private readonly ILogger<StockMovementsController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly App.DTO.v1.Mappers.StockMovementAPIMapper _mapper =
            new App.DTO.v1.Mappers.StockMovementAPIMapper();

        public StockMovementsController(IAppBLL bll, ILogger<StockMovementsController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<App.DTO.v1.StockMovement> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.StockMovement>>> GetActions()
        {
            return (await _bll.StockMovementService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.StockMovement>> GetActionEntity(Guid id)
        {
            var stockMovement = await _bll.StockMovementService.FindAsync(id);

            if (stockMovement == null)
            {
                return NotFound();
            }

            return _mapper.Map(stockMovement)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, App.DTO.v1.StockMovement stockMovement)
        {
            if (id != stockMovement.Id)
            {
                return BadRequest();
            }

            await _bll.StockMovementService.UpdateAsync(_mapper.Map(stockMovement)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.StockMovement>> PostActionEntity(App.DTO.v1.StockMovement stockMovement)
        {
            var bllEntity = _mapper.Map(stockMovement);
            _bll.StockMovementService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetStockMovement", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, stockMovement);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.StockMovementService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
