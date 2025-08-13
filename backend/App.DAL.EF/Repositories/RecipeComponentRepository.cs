using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying recipe component data from the database.
/// Supports retrieval of components (ingredients) for a given recipe product.
/// </summary>
public class RecipeComponentRepository: BaseRepository<DAL.DTO.RecipeComponent, Domain.Logic.RecipeComponent>, IRecipeComponentRepository
{
    public RecipeComponentRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new RecipeComponentUowMapper())
    {
    }
    
    /// <summary>
    /// Retrieves all component entries (ingredients) for the specified recipe product.
    /// Returns domain-level RecipeComponent entities.
    /// </summary>
    public async Task<List<Domain.Logic.RecipeComponent>> GetComponentsByRecipeProductIdAsync(Guid productId)
    {
        return await RepositoryDbSet
            .Where(rc => rc.ProductRecipeId == productId)
            .ToListAsync();
    }
    
    /// <summary>
    /// Retrieves all recipe component entries.
    /// </summary>
    public async Task<IEnumerable<DAL.DTO.RecipeComponent?>> GetEnrichedRecipeComponents()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.ProductRecipe)
            .Include(a => a.ComponentProduct)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}