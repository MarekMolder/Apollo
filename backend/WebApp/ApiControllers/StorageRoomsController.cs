using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize(Roles = "admin,manager,tootaja")]
    public class StorageRoomsController : ControllerBase
    {
        private readonly ILogger<StorageRoomsController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly App.DTO.v1.Mappers.StorageRoomAPIMapper _mapper =
            new App.DTO.v1.Mappers.StorageRoomAPIMapper();

        public StorageRoomsController(IAppBLL bll, ILogger<StorageRoomsController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<App.DTO.v1.StorageRoom> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.StorageRoom>>> GetActions()
        {
            return (await _bll.StorageRoomService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.StorageRoom>> GetActionEntity(Guid id)
        {
            var storageRoom = await _bll.StorageRoomService.FindAsync(id);

            if (storageRoom == null)
            {
                return NotFound();
            }

            return _mapper.Map(storageRoom)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, App.DTO.v1.StorageRoom storageRoom)
        {
            if (id != storageRoom.Id)
            {
                return BadRequest();
            }

            await _bll.StorageRoomService.UpdateAsync(_mapper.Map(storageRoom)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.StorageRoom>> PostActionEntity(App.DTO.v1.StorageRoom storageRoom)
        {
            var bllEntity = _mapper.Map(storageRoom);
            _bll.StorageRoomService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetStorageRoom", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, storageRoom);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.StorageRoomService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
        
        /// <summary>
        /// Get all storage rooms that belong to an inventory
        /// </summary>
        /// <param name="inventoryId">Inventory ID</param>
        /// <returns>List of storage rooms</returns>
        [HttpGet("inventory/{inventoryId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<App.DTO.v1.StorageRoom>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.StorageRoom>>> GetByInventory(Guid inventoryId)
        {
            var result = await _bll.StorageRoomService.GetAllByInventoryIdAsync(inventoryId);
    
            if (!result.Any()) return NotFound("No storage rooms found for this inventory.");

            return Ok(result.Select(x => _mapper.Map(x)!));
        }
        
        [HttpGet("byinventory/{inventoryId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<App.DTO.v1.StorageRoom>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.StorageRoom>>> GetByInventory2(Guid inventoryId)
        {
            // 1) leia inventar
            var inv = await _bll.InventoryService.FindAsync(inventoryId);
            if (inv == null) return NotFound();

            // 2) kas kasutaja roll kattub inventari AllowedRoles’iga?
            var userRoles = User.Claims
                .Where(c => c.Type is ClaimTypes.Role or "role")
                .Select(c => c.Value)
                .ToList();

            if (inv.AllowedRoles == null || !inv.AllowedRoles.Intersect(userRoles).Any())
                return Forbid();

            // 3) ruumid
            var rooms = await _bll.StorageRoomService.GetAllByInventoryIdAsync(inventoryId);
            return Ok(rooms.Select(r => _mapper.Map(r)!));
        }

    }
}
