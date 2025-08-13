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
    /// Api controller for managing products.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,manager,tootaja")]
    public class ProductsController : ControllerBase
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ProductsController> _logger;
        
        private readonly ProductApiMapper _mapper = new();
        
        private readonly EnrichedProductApiMapper _enrichedProductApiMapper = new();

        public ProductsController(IAppBll bll, ILogger<ProductsController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<Product> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Fetching all products");
            var result = (await _bll.ProductService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
            
            _logger.LogInformation("Returned {Count} products", result.Count);
            return result;        }

        /// <summary>
        /// Get product by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            _logger.LogInformation("Fetching product with ID {Id}", id);
            var product = await _bll.ProductService.FindAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with ID {Id} not found", id);
                return NotFound();
            }

            return _mapper.Map(product)!;
        }

        /// <summary>
        /// Update a product.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                _logger.LogWarning("PutProduct failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, product.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating product with ID {Id}", id);
            await _bll.ProductService.UpdateAsync(_mapper.Map(product)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _logger.LogInformation("Creating new product");
            var bllEntity = _mapper.Map(product);
            _bll.ProductService.Add(bllEntity);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Created product with ID {Id}", bllEntity.Id);

            return CreatedAtAction("GetProduct", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, product);
        }

        /// <summary>
        /// Delete a product by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            _logger.LogInformation("Deleting product with ID {Id}", id);
            await _bll.ProductService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted product with ID {Id}", id);
            return NoContent();
        }
        
        /// <summary>
        /// Get enriched product data.
        /// </summary>
        [HttpGet("enrichedProducts/")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedProduct>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedProduct>>> GetEnrichedProduct()
        {
            _logger.LogInformation("Fetching enriched product data");
            var data = await _bll.ProductService.GetEnrichedProducts();
            var res = data.Select(u => _enrichedProductApiMapper.Map(u)!).ToList();
            
            _logger.LogInformation("Returned {Count} enriched products", res.Count);
            return Ok(res);
        }
        
        /// <summary>
        /// Get products by supplier.
        /// </summary>
        [HttpGet("by-supplier/{supplierId}")]
        [ProducesResponseType(typeof(IEnumerable<EnrichedProduct>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EnrichedProduct>>> GetProductsBySupplier(Guid supplierId)
        {
            _logger.LogInformation("Fetching products for supplier ID {SupplierId}", supplierId);

            var products = await _bll.ProductService.GetProductsBySupplierAsync(supplierId);

            var result = products.Select(p => _enrichedProductApiMapper.Map(p)!).ToList();

            _logger.LogInformation("Returned {Count} products for supplier ID {SupplierId}", result.Count, supplierId);

            return Ok(result);
        }
    }
}
