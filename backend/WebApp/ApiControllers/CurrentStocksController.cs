using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Contracts;
using App.DTO.v1;
using App.DTO.v1.ApiEntities;
using App.DTO.v1.ApiMappers;
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
    public class CurrentStocksController : ControllerBase
    {
        private readonly ILogger<CurrentStocksController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly App.DTO.v1.Mappers.CurrentStockAPIMapper _mapper =
            new App.DTO.v1.Mappers.CurrentStockAPIMapper();
        
        private readonly EnrichedCurrentStockApiMapper _enrichedCurrentStockApiMapper = new();

        public CurrentStocksController(IAppBLL bll, ILogger<CurrentStocksController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<App.DTO.v1.CurrentStock> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.CurrentStock>>> GetActions()
        {
            return (await _bll.CurrentStockService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.CurrentStock>> GetActionEntity(Guid id)
        {
            var currentStock = await _bll.CurrentStockService.FindAsync(id);

            if (currentStock == null)
            {
                return NotFound();
            }

            return _mapper.Map(currentStock)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, App.DTO.v1.CurrentStock currentStock)
        {
            if (id != currentStock.Id)
            {
                return BadRequest();
            }

            await _bll.CurrentStockService.UpdateAsync(_mapper.Map(currentStock)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.CurrentStock>> PostActionEntity(App.DTO.v1.CurrentStock currentStock)
        {
            var bllEntity = _mapper.Map(currentStock);
            _bll.CurrentStockService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetCurrentStock", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, currentStock);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.CurrentStockService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpGet("bystorageroom/{id}")]
        [ProducesResponseType(typeof(IEnumerable<App.DTO.v1.ApiEntities.EnrichedCurrentStock>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.ApiEntities.EnrichedCurrentStock>>> GetByStorageRoom(Guid id)
        {
            var stocks = await _bll.CurrentStockService.GetByStorageRoomIdAsync(id);
            var res = stocks.Select(x => _enrichedCurrentStockApiMapper.Map(x)!);
            return Ok(res);
            
        }
        
        // API
        [HttpGet("lowestStockProducts")]
        public async Task<IActionResult> GetLowestStockProducts()
        {
            var result = await _bll.CurrentStockService.GetLowestStockProductsAsync(5);

            var response = result.Select(x => new {
                x.ProductId,
                x.ProductName,
                x.Quantity
            });

            return Ok(response);
        }
        
        /// <summary>
        /// Get total monetary value of all current stock (optionally filtered by Inventory ID)
        /// </summary>
        /// <param name="inventoryId">Optional Inventory ID</param>
        /// <returns>Total value in EUR</returns>
        [HttpGet("totalWorth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalInventoryWorth([FromQuery] Guid? inventoryId)
        {
            var total = await _bll.CurrentStockService.GetTotalInventoryWorthAsync(inventoryId);
            return Ok(total);
        }
    }
}
