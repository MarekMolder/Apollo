using System.Security.Claims;
using App.BLL.Contracts;
using App.DTO.v1;
using App.DTO.v1.ApiEntities;
using App.DTO.v1.ApiMapper;
using App.DTO.v1.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        
        private readonly StorageRoomAPIMapper _mapper = new();
        private readonly EnrichedStorageRoomApiMapper _enrichedMapper = new();

        public StorageRoomsController(IAppBLL bll, ILogger<StorageRoomsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

    // ----------------------------------------------------------------
    //  HELPER – loe kasutaja rollid JA LOGI need välja
    // ----------------------------------------------------------------
    private List<string> GetCurrentUserRoles()
    {
        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        _logger.LogInformation("▶ JWT roles for user {User}: {Roles}",
            User.Identity?.Name ?? "anonymous",
            roles.Count == 0 ? "(none)" : string.Join(", ", roles));

        return roles;
    }

    // ----------------------------------------------------------------
    //  GET /inventories
    // ----------------------------------------------------------------
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<StorageRoom>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StorageRoom>>> GetStorageRooms()
    {
        var userRoles = GetCurrentUserRoles();

        var all = (await _bll.StorageRoomService.AllAsync()).ToList();

        var filtered = all
            .Where(i => i.AllowedRoles != null && i.AllowedRoles.Intersect(userRoles).Any())
            .ToList();

        return filtered.Select(i => _mapper.Map(i)!).ToList();
    }

    // ----------------------------------------------------------------
    //  GET /inventories/enrichedInventories
    // ----------------------------------------------------------------
    [HttpGet("enrichedStorageRooms")]
    [ProducesResponseType(typeof(IEnumerable<EnrichedStorageRoom>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EnrichedStorageRoom>>> GetEnrichedStorageRooms()
    {
        var userRoles = GetCurrentUserRoles();

        var all = (await _bll.StorageRoomService.GetEnrichedStorageRooms()).ToList();

        var filtered = all
            .Where(i => i!.AllowedRoles != null && i.AllowedRoles.Intersect(userRoles).Any())
            .ToList();

        return Ok(filtered.Select(i => _enrichedMapper.Map(i)!));
    }

    // ----------------------------------------------------------------
    //  Ülejäänud meetodid jäid samaks (lühendatud)
    // ----------------------------------------------------------------
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<StorageRoom>> GetInventory(Guid id)
    {
        var inv = await _bll.StorageRoomService.FindAsync(id);
        if (inv == null) return NotFound();

        if (inv.AllowedRoles == null ||
            !inv.AllowedRoles.Intersect(GetCurrentUserRoles()).Any())
            return Forbid();     // nähtav vaid lubatud rollidele

        return _mapper.Map(inv)!;
    }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, StorageRoom storageRoom)
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
        public async Task<ActionResult<StorageRoom>> PostActionEntity(StorageRoom storageRoom)
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
    }
}
