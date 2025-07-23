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
    public class StorageRoomInInventoriesController : ControllerBase
    {
        private readonly ILogger<StorageRoomInInventoriesController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly App.DTO.v1.Mappers.StorageRoomInInventoryAPIMapper _mapper =
            new App.DTO.v1.Mappers.StorageRoomInInventoryAPIMapper();

        public StorageRoomInInventoriesController(IAppBLL bll, ILogger<StorageRoomInInventoriesController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<App.DTO.v1.StorageRoomInInventory> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.StorageRoomInInventory>>> GetActions()
        {
            return (await _bll.StorageRoomInInventoryService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.StorageRoomInInventory>> GetActionEntity(Guid id)
        {
            var storageRoomInInventory = await _bll.StorageRoomInInventoryService.FindAsync(id);

            if (storageRoomInInventory == null)
            {
                return NotFound();
            }

            return _mapper.Map(storageRoomInInventory)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, App.DTO.v1.StorageRoomInInventory storageRoomInInventory)
        {
            if (id != storageRoomInInventory.Id)
            {
                return BadRequest();
            }

            await _bll.StorageRoomInInventoryService.UpdateAsync(_mapper.Map(storageRoomInInventory)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.StorageRoomInInventory>> PostActionEntity(App.DTO.v1.StorageRoomInInventory storageRoomInInventory)
        {
            var bllEntity = _mapper.Map(storageRoomInInventory);
            _bll.StorageRoomInInventoryService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetStorageRoomInInventory", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, storageRoomInInventory);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.StorageRoomInInventoryService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
