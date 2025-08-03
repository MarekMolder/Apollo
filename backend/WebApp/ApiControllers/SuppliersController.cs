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
    /// Api controller for managing suppliers.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager,tootaja")]
    public class SuppliersController : ControllerBase
    { 
        private readonly IAppBll _bll;
        
        private readonly ILogger<SuppliersController> _logger;
        
        private readonly SupplierApiMapper _mapper = new();
        
        private readonly EnrichedSupplierApiMapper _enrichedSupplierApiMapper = new();

        public SuppliersController(IAppBll bll, ILogger<SuppliersController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all suppliers
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<Supplier> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            _logger.LogInformation("Fetching all suppliers");
            var result = (await _bll.SupplierService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
            
            _logger.LogInformation("Returned {Count} suppliers", result.Count);
            return result;        }

        /// <summary>
        /// Get supplier by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(Guid id)
        {
            _logger.LogInformation("Fetching supplier with ID {Id}", id);
            var supplier = await _bll.SupplierService.FindAsync(id);

            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found", id);
                return NotFound();
            }

            return _mapper.Map(supplier)!;
        }

        /// <summary>
        /// Update supplier
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(Guid id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                _logger.LogWarning("PutSupplier failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, supplier.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating supplier with ID {Id}", id);
            await _bll.SupplierService.UpdateAsync(_mapper.Map(supplier)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new supplier
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
            _logger.LogInformation("Creating new supplier");
            var bllEntity = _mapper.Map(supplier);
            _bll.SupplierService.Add(bllEntity);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Created supplier with ID {Id}", bllEntity.Id);

            return CreatedAtAction("GetSupplier", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, supplier);
        }

        /// <summary>
        /// Delete supplier by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(Guid id)
        {
            _logger.LogInformation("Deleting supplier with ID {Id}", id);
            await _bll.SupplierService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted supplier with ID {Id}", id);
            return NoContent();
        }
        
        /// <summary>
        /// Get enriched suppliers
        /// </summary>
        [HttpGet("enrichedSuppliers/")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedSupplier>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedSupplier>>> GetEnrichedSuppliers()
        {
            _logger.LogInformation("Fetching enriched suppliers");
            var data = await _bll.SupplierService.GetEnrichedSuppliers();
            var res = data.Select(u => _enrichedSupplierApiMapper.Map(u)!).ToList();
            
            _logger.LogInformation("Returned {Count} enriched suppliers", res.Count);
            return Ok(res);
        }
        
        /// <summary>
        /// Get all products offered by the specified supplier.
        /// </summary>
        [HttpGet("{id}/products")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsBySupplier(Guid id)
        {
            _logger.LogInformation("Fetching products for supplier ID {Id}", id);
    
            var products = await _bll.ProductService.GetProductsBySupplierAsync(id);
    
            if (!products.Any())
            {
                _logger.LogInformation("No products found for supplier ID {Id}", id);
                return Ok(new List<Product>());
            }

            var result = products.Select(p => new ProductApiMapper().Map(p)!);
            return Ok(result);
        }
    }
}
