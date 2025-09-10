using App.BLL.Contracts;
using App.DTO.v1;
using App.DTO.v1.Mappers;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// Api controller for managing products categories.
    /// </summary>
    [ApiVersion( "1.0" )]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "admin,juhataja,töötaja")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IAppBll _bll;
        
        private readonly ILogger<ProductCategoriesController> _logger;
        
        private readonly ProductCategoryApiMapper _mapper = new();

        public ProductCategoriesController(IAppBll bll, ILogger<ProductCategoriesController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        /// <summary>
        /// Get all product categories.
        /// </summary>
        [HttpGet]
        [Produces( "application/json" )]
        [ProducesResponseType( typeof( IEnumerable<ProductCategory> ), 200 )]
        [ProducesResponseType( 404 )]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
        {
            _logger.LogInformation("Fetching all product categories");
            var result = (await _bll.ProductCategoryService.AllAsync()).Select(x => _mapper.Map(x)!).ToList();
            
            _logger.LogInformation("Returned {Count} product categories", result.Count);
            return result;        }

        /// <summary>
        /// Get product category by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(Guid id)
        {
            _logger.LogInformation("Fetching product category with ID {Id}", id);
            var productCategory = await _bll.ProductCategoryService.FindAsync(id);

            if (productCategory == null)
            {
                _logger.LogWarning("Product category with ID {Id} not found", id);
                return NotFound();
            }

            return _mapper.Map(productCategory)!;
        }

        /// <summary>
        /// Update product category.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCategory(Guid id, ProductCategory productCategory)
        {
            if (id != productCategory.Id)
            {
                _logger.LogWarning("PutProductCategory failed: ID mismatch (URL: {Id}, Body: {EntityId})", id, productCategory.Id);
                return BadRequest();
            }

            _logger.LogInformation("Updating product category with ID {Id}", id);
            await _bll.ProductCategoryService.UpdateAsync(_mapper.Map(productCategory)!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create new product category.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        {
            _logger.LogInformation("Creating new product category");
            var bllEntity = _mapper.Map(productCategory);
            _bll.ProductCategoryService.Add(bllEntity);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Created product category with ID {Id}", bllEntity.Id);
            
            return CreatedAtAction("GetProductCategory", new
            {
                id = bllEntity.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString()
            }, productCategory);
        }

        /// <summary>
        /// Delete product category by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory(Guid id)
        {
            _logger.LogInformation("Deleting product category with ID {Id}", id);
            await _bll.ProductCategoryService.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            
            _logger.LogInformation("Deleted product category with ID {Id}", id);
            return NoContent();
        }
    }
}
