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

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Api controller for managing ActionEntity records.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager,tootaja")]
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
            var isAdmin = User.IsInRole("admin") || User.IsInRole("manager");
            _logger.LogInformation("Fetching actions for user {UserId} (isAdmin: {IsAdmin})", User.GetUserId(), isAdmin);
            
            var actions = isAdmin
                ? await _bll.ActionEntityService.AllAsync()
                : await _bll.ActionEntityService.AllAsync(User.GetUserId());

            return actions.Select(x => _mapper.Map(x)!).ToList();
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
                _logger.LogInformation("Updating status of action {Id} to {Status}", id, dto.Status);
                var updated = await _bll.ActionEntityService.UpdateStatusAsync(id, dto.Status);
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
            _logger.LogInformation("Fetching enriched action data with filters user={User} month={Month} year={Year} status={Status}",
                userEmail, month, year, status);

            var data = await _bll.ActionEntityService.GetEnrichedActionEntitiesFiltered(userEmail, month, year, status);
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

            var result = await _bll.ActionEntityService.GetTopRemovedProductsAsync();

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

            var result = await _bll.ActionEntityService.GetTopUsersByRemovedQuantityAsync();

            var response = result.Select(r => new
            {
                r.CreatedBy, r.TotalRemovals
            });

            return Ok(response);
        }
    }
}
