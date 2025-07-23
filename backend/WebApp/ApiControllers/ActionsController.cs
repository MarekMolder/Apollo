using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.Contracts;
using App.DTO.v1.ApiMappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager,tootaja")]
    public class ActionsController : ControllerBase
    {
        private readonly ILogger<ActionsController> _logger;
        private readonly IAppBLL _bll;
        
        private readonly App.DTO.v1.Mappers.ActionEntityAPIMapper _mapper =
            new App.DTO.v1.Mappers.ActionEntityAPIMapper();

        private readonly EnrichedActionEntityApiMapper _enrichedActionEntityApiMapper = new();

        public ActionsController(IAppBLL bll, ILogger<ActionsController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<App.DTO.v1.ActionEntity> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.ActionEntity>>> GetActions()
        {
            var isAdmin = User.IsInRole("admin") || User.IsInRole("manager");

            var actions = isAdmin
                ? await _bll.ActionEntityService.AllAsync() // admin näeb kõiki
                : await _bll.ActionEntityService.AllAsync(User.GetUserId()); // tavakasutaja ainult enda

            return actions.Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DTO.v1.ActionEntity>> GetActionEntity(Guid id)
        {
            var actionEntity = await _bll.ActionEntityService.FindAsync(id);

            if (actionEntity == null)
            {
                return NotFound();
            }

            return _mapper.Map(actionEntity)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, App.DTO.v1.ActionEntity actionEntity)
        {
            if (id != actionEntity.Id)
            {
                return BadRequest();
            }

            await _bll.ActionEntityService.UpdateAsync(_mapper.Map(actionEntity)!, User.GetUserId());
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<App.DTO.v1.ActionEntity>> PostActionEntity(App.DTO.v1.ActionEntity actionEntity)
        {
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
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
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
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] App.DTO.v1.StatusUpdateDto dto)
        {
            try
            {
                var updated = await _bll.ActionEntityService.UpdateStatusAsync(id, dto.Status);
                if (!updated) return NotFound();

                await _bll.SaveChangesAsync();
                return Ok(new { message = $"Status updated to {dto.Status}" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("enrichedAction/")]
        [ProducesResponseType(typeof(IEnumerable<App.DTO.v1.ApiEntities.EnrichedActionEntity>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<App.DTO.v1.ApiEntities.EnrichedActionEntity>>> GetEnrichedActionEntities()
        {
            var data = await _bll.ActionEntityService.GetEnrichedActionEntities();

            var res = data.Select(u => _enrichedActionEntityApiMapper.Map(u)!).ToList();
            return Ok(res);
        }
        
        /// <summary>
        /// Get top 5 most frequently removed products (Accepted & Remove actions)
        /// </summary>
        /// <returns>List of product name, ID, and removal count</returns>
        [HttpGet("problematicProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetTopRemovedProducts()
        {
            var result = await _bll.ActionEntityService.GetTopRemovedProductsAsync();

            var response = result.Select(r => new
            {
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                RemoveQuantity = r.RemoveQuantity
            });

            return Ok(response);
        }
        
        /// <summary>
        /// Get top 5 users who removed the most products (sum of quantity)
        /// </summary>
        /// <returns>List of users and total quantity removed</returns>
        [HttpGet("topUsersByRemove")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetTopUsersByRemovedQuantity()
        {
            var result = await _bll.ActionEntityService.GetTopUsersByRemovedQuantityAsync();

            var response = result.Select(r => new
            {
                CreatedBy = r.CreatedBy,
                TotalRemovedQuantity = r.TotalRemovedQuantity
            });

            return Ok(response);
        }
        
    }
}
