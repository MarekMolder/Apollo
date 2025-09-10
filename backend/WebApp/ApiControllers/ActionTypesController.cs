using App.BLL.Contracts;
using App.DTO.v1.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ActionTypeEntity = App.DTO.v1.ActionTypeEntity;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Api controller for managing Action Types.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,juhataja,töötaja")]
    
    public class ActionTypesController : ControllerBase
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ActionTypesController> _logger;
        
        private readonly ActionTypeEntityApiMapper _mapper = new();

        public ActionTypesController(IAppBll bll, ILogger<ActionTypesController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Gets all Action Types.
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<ActionTypeEntity> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<ActionTypeEntity>>> GetActionTypes()
        {
            _logger.LogInformation("Fetching all Action Types");

            var result = (await _bll.ActionTypeEntityService.AllAsync())
                .Select(x => _mapper.Map(x)!).ToList();

            _logger.LogInformation("Returned {Count} Action Types", result.Count);
            return result;
        }

        /// <summary>
        /// Gets a single Action Type by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ActionTypeEntity>> GetActionType(Guid id)
        {
            _logger.LogInformation("Fetching ActionType with ID {Id}", id);

            var actionTypeEntity = await _bll.ActionTypeEntityService.FindAsync(id);

            if (actionTypeEntity == null)
            {
                _logger.LogWarning("ActionType with ID {Id} not found", id);
                return NotFound();
            }

            return _mapper.Map(actionTypeEntity)!;
        }

        /// <summary>
        /// Updates an existing Action Type.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionType(Guid id, ActionTypeEntity actionTypeEntity)
        {
            if (id != actionTypeEntity.Id)
            {
                _logger.LogWarning("PutActionEntity failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, actionTypeEntity.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating ActionType {Id}", id);

            await _bll.ActionTypeEntityService.UpdateAsync(_mapper.Map(actionTypeEntity)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Creates a new Action Type.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ActionTypeEntity>> PostActionType(ActionTypeEntity actionTypeEntity)
        {
            _logger.LogInformation("Creating new ActionType");

            var bllEntity = _mapper.Map(actionTypeEntity);
            _bll.ActionTypeEntityService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            _logger.LogInformation("Created ActionType with ID {Id}", bllEntity.Id);

            return CreatedAtAction("GetActionType", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, actionTypeEntity);
        }

        /// <summary>
        /// Deletes an Action Type by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionType(Guid id)
        {
            _logger.LogInformation("Deleting ActionType with ID {Id}", id);

            await _bll.ActionTypeEntityService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted ActionType with ID {Id}", id);
            return NoContent();
        }
    }
}
