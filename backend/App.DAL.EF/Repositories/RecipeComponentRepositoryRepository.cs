using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RecipeComponentRepositoryRepository: BaseRepository<RecipeComponent, Domain.Logic.RecipeComponent>, IRecipeComponentRepository
{
    public RecipeComponentRepositoryRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new RecipeComponentUOWMapper())
    {
    }
    
    public async Task<List<Domain.Logic.RecipeComponent>> GetComponentsByRecipeProductIdAsync(Guid productId)
    {
        return await RepositoryDbSet
            .Where(rc => rc.ProductRecipeId == productId)
            .ToListAsync();
    }
}