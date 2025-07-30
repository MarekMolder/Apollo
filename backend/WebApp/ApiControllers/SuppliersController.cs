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
    public class SuppliersController : ControllerBase
    {
        private readonly ILogger<SuppliersController> _logger;
        private readonly IAppBll _bll;
        
        private readonly SupplierApiMapper _mapper = new();
        
        private readonly EnrichedSupplierApiMapper _enrichedSupplierApiMapper = new();

        public SuppliersController(IAppBll bll, ILogger<SuppliersController> logger)
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
        [ProducesResponseType( typeof( IEnumerable<Supplier> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetActions()
        {
            return (await _bll.SupplierService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
        }

        /// <summary>
        /// Get person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetActionEntity(Guid id)
        {
            var supplier = await _bll.SupplierService.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return _mapper.Map(supplier)!;
        }

        /// <summary>
        /// Update person
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActionEntity(Guid id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest();
            }

            await _bll.SupplierService.UpdateAsync(_mapper.Map(supplier)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostActionEntity(Supplier supplier)
        {
            var bllEntity = _mapper.Map(supplier);
            _bll.SupplierService.Add(bllEntity);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetSupplier", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, supplier);
        }

        /// <summary>
        /// Delete person by id - owned by current user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActionEntity(Guid id)
        {
            await _bll.SupplierService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpGet("enrichedSuppliers/")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedSupplier>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedSupplier>>> GetEnrichedSuppliers()
        {
            var data = await _bll.SupplierService.GetEnrichedSuppliers();

            var res = data.Select(u => _enrichedSupplierApiMapper.Map(u)!).ToList();
            return Ok(res);
        }
    }
}
