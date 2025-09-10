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
    /// <summary>
    /// Api controller for managing storage rooms with role-based access.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,juhataja,töötaja")]
    public class StorageRoomsController : ControllerBase
    {
        private readonly IAppBll _bll;

        private readonly ILogger<StorageRoomsController> _logger;

        private readonly StorageRoomApiMapper _mapper = new();

        private readonly EnrichedStorageRoomApiMapper _enrichedMapper = new();

        public StorageRoomsController(IAppBll bll, ILogger<StorageRoomsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all storage rooms allowed for the current user's roles.
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<StorageRoom>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StorageRoom>>> GetStorageRooms()
        {
            _logger.LogInformation("Fetching storage rooms based on user roles");
            var userRoles = GetCurrentUserRoles();

            var all = (await _bll.StorageRoomService.AllAsync()).ToList();

            var filtered = all
                .Where(i => i.AllowedRoles != null && i.AllowedRoles.Intersect(userRoles).Any())
                .ToList();

            _logger.LogInformation("Returned {Count} storage rooms", filtered.Count);
            return filtered.Select(i => _mapper.Map(i)!).ToList();
        }

        /// <summary>
        /// Get enriched storage rooms allowed for current user.
        /// </summary>
        [HttpGet("enrichedStorageRooms")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedStorageRoom>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedStorageRoom>>> GetEnrichedStorageRooms()
        {
            _logger.LogInformation("Fetching enriched storage rooms based on user roles");
            var userRoles = GetCurrentUserRoles();

            var all = (await _bll.StorageRoomService.GetEnrichedStorageRooms()).ToList();

            var filtered = all
                .Where(i => i!.AllowedRoles != null && i.AllowedRoles.Intersect(userRoles).Any())
                .ToList();

            _logger.LogInformation("Returned {Count} enriched storage rooms", filtered.Count);
            return Ok(filtered.Select(i => _enrichedMapper.Map(i)!));
        }

        /// <summary>
        /// Get a storage room by ID if user has access.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StorageRoom>> GetStorageRoom(Guid id)
        {
            _logger.LogInformation("Fetching storage room with ID {Id}", id);
            var storageRoom = await _bll.StorageRoomService.FindAsync(id);

            if (storageRoom == null)
            {
                _logger.LogWarning("Storage room with ID {Id} not found", id);
                return NotFound();
            }

            if (storageRoom.AllowedRoles == null ||
                !storageRoom.AllowedRoles.Intersect(GetCurrentUserRoles()).Any())
            {
                _logger.LogWarning("Access denied to storage room ID {Id}", id);
                return Forbid();
            }

            return _mapper.Map(storageRoom)!;
        }

        /// <summary>
        /// Update a storage room.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStorageRoom(Guid id, StorageRoom storageRoom)
        {
            if (id != storageRoom.Id)
            {
                _logger.LogWarning("PutStorageRoom failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, storageRoom.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating storage room with ID {Id}", id);
            await _bll.StorageRoomService.UpdateAsync(_mapper.Map(storageRoom)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create a new storage room.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<StorageRoom>> PostStorageRoom(StorageRoom storageRoom)
        {
            _logger.LogInformation("Creating new storage room");
            var bllEntity = _mapper.Map(storageRoom);
            _bll.StorageRoomService.Add(bllEntity);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Created storage room with ID {Id}", bllEntity.Id);

            return CreatedAtAction("GetStorageRoom", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, storageRoom);
        }

        /// <summary>
        /// Delete a storage room by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStorageRoom(Guid id)
        {
            _logger.LogInformation("Deleting storage room with ID {Id}", id);
            await _bll.StorageRoomService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted storage room with ID {Id}", id);
            return NoContent();
        }

        /// <summary>
        /// Gets current user's roles from JWT and logs them.
        /// </summary>
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
    }
}
