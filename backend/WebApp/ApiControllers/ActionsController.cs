using System.Security.Claims;
using App.BLL.Contracts;
using App.DTO.v1;
using App.DTO.v1.ApiEntities;
using App.DTO.v1.ApiMapper;
using App.DTO.v1.Mappers;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Api controller for managing ActionEntity records.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,juhataja,töötaja")]
    public class ActionsController : ControllerBase
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ActionsController> _logger;
        
        private readonly ActionEntityApiMapper _mapper = new();

        private readonly EnrichedActionEntityApiMapper _enrichedActionEntityApiMapper = new();

        public ActionsController(IAppBll bll, ILogger<ActionsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Gets all actions. Returns all if admin/manager, or own actions if regular user.
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<ActionEntity> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<ActionEntity>>> GetActions()
        {
            var roles = GetCurrentUserRoles();
            var isAdmin = roles.Any(r =>
                r.Equals("admin", StringComparison.OrdinalIgnoreCase));

            _logger.LogInformation("Fetching actions for user {UserId} (isAdmin: {IsAdmin})", User.GetUserId(), isAdmin);

            var data = await _bll.ActionEntityService.GetEnrichedActionEntities();

            if (!isAdmin)
            {
                data = data.Where(a =>
                    a?.StorageRoom?.AllowedRoles != null &&
                    a.StorageRoom.AllowedRoles.Intersect(roles, StringComparer.OrdinalIgnoreCase).Any());
            }
            
            return Ok(data.Select(x => _mapper.Map(x)!).ToList());
        }

        /// <summary>
        /// Gets one action by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ActionEntity>> GetActionEntity(Guid id)
        {
            _logger.LogInformation("Getting action with ID {Id}", id);
            var actionEntity = await _bll.ActionEntityService.FindAsync(id);

            if (actionEntity == null)
            {
                _logger.LogWarning("Action with ID {Id} not found", id);
                return NotFound();
            }

            var roles = GetCurrentUserRoles();
            var isAdmin = roles.Any(r =>
                r.Equals("admin", StringComparison.OrdinalIgnoreCase));

            if (!isAdmin)
            {
                var allowed = actionEntity.StorageRoom?.AllowedRoles != null &&
                              actionEntity.StorageRoom.AllowedRoles.Intersect(roles, StringComparer.OrdinalIgnoreCase).Any();

                if (!allowed)
                {
                    _logger.LogWarning("Access denied to action {Id} for user {User}", id, User.Identity?.Name);
                    return Forbid();
                }
            }

            return _mapper.Map(actionEntity)!;
        }
        
        /// <summary>
        /// Updates an existing action.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, ActionEntity actionEntity)
        {
            if (id != actionEntity.Id)
            {
                _logger.LogWarning("PutActionEntity failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, actionEntity.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating action {Id} by user {UserId}", id, User.GetUserId());

            await _bll.ActionEntityService.UpdateAsync(_mapper.Map(actionEntity)!, User.GetUserId());
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Creates a new action.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ActionEntity>> PostActionEntity(ActionEntity actionEntity)
        {
            _logger.LogInformation("Creating new action for user {UserId}", User.GetUserId());

            var bllEntity = _mapper.Map(actionEntity);
            _bll.ActionEntityService.Add(bllEntity, User.GetUserId());
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetAction", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, actionEntity);
        }

        /// <summary>
        /// Deletes an action by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            _logger.LogInformation("Deleting action {Id} by user {UserId}", id, User.GetUserId());

            await _bll.ActionEntityService.RemoveAsync(id, User.GetUserId());
            await _bll.SaveChangesAsync();
            return NoContent();
        }
        
        /// <summary>
        /// Update status of an ActionEntity (Accepted / Declined)
        /// </summary>
        /// <param name="id">Action ID</param>
        /// <param name="dto">New status</param>
        /// <returns></returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] StatusUpdateDto dto)
        {
            try
            {
                var currentUser = User.GetUserEmail();
                var roles = GetCurrentUserRoles();
                
                _logger.LogInformation("Updating status of action {Id} to {Status}", id, dto.Status, currentUser);
                var updated = await _bll.ActionEntityService.UpdateStatusAsync(id, dto.Status, currentUser!, roles);
                if (!updated)
                {
                    _logger.LogWarning("Action with ID {Id} not found for status update", id);
                    return NotFound();
                }

                await _bll.SaveChangesAsync();
                return Ok(new { message = $"Status updated to {dto.Status}" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Finalized status update attempt for action {Id}: {Message}", id, ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid status update for action {Id}: {Message}", id, ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }
        
        /// <summary>Gets enriched view of all actions with optional filters.</summary>
        [HttpGet("enrichedAction")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedActionEntity>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedActionEntity>>> GetEnrichedActionEntities(
            [FromQuery] string? userEmail,
            [FromQuery] int? month,
            [FromQuery] int? year,
            [FromQuery] string? status
        )
        {
            var roles = GetCurrentUserRoles();
            var isAdmin = roles.Any(r => r.Equals("admin", StringComparison.OrdinalIgnoreCase));
            var isWorker = roles.Any(r => r.Equals("töötaja", StringComparison.OrdinalIgnoreCase));

            _logger.LogInformation("Fetching enriched action data with filters user={User} month={Month} year={Year} status={Status}",
                userEmail, month, year, status);

            var data = await _bll.ActionEntityService.GetEnrichedActionEntitiesFiltered(userEmail, month, year, status);
            
            
            if (isWorker)
            {
                var currentUser = User.GetUserEmail(); // tee util, mis võtab emaili claimist
                data = data.Where(a => a!.CreatedBy == currentUser);
            }
            else if (!isAdmin)
            {
                data = data.Where(a =>
                    a?.StorageRoom?.AllowedRoles != null &&
                    a.StorageRoom.AllowedRoles.Intersect(roles, StringComparer.OrdinalIgnoreCase).Any());
            }

            var res = data.Select(u => _enrichedActionEntityApiMapper.Map(u)!).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Gets top 5 most frequently removed products.
        /// </summary>
        [HttpGet("problematicProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetTopRemovedProducts()
        {
            _logger.LogInformation("Fetching top removed products");

            var roles = GetCurrentUserRoles();
            var isAdmin = roles.Contains("admin", StringComparer.OrdinalIgnoreCase);

            var result = await _bll.ActionEntityService
                .GetTopRemovedProductsAsync(isAdmin ? null : roles);

            var response = result.Select(r => new
            {
                r.ProductId,
                r.ProductName,
                r.RemoveQuantity,
                r.ProductUnit,
                r.ProductVolume,
                r.ProductVolumeUnit
            });

            return Ok(response);
        }
        
        /// <summary>
        /// Gets top 5 users who removed the most products.
        /// </summary>
        [HttpGet("topUsersByRemove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetTopUsersByRemovedQuantity()
        {
            _logger.LogInformation("Fetching top users by removed product quantity");

            var roles = GetCurrentUserRoles();
            var isAdmin = roles.Contains("admin", StringComparer.OrdinalIgnoreCase);

            var result = await _bll.ActionEntityService
                .GetTopUsersByRemovedQuantityAsync(isAdmin ? null : roles);

            _logger.LogInformation("▶ Got {Count} top users", result.Count);

            var response = result.Select(r => new
            {
                r.CreatedBy,
                r.TotalRemovals
            });

            return Ok(response);
        }
        
        /// <summary>
        /// Helper function to get current user roles.
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
