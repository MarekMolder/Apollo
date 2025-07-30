using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying supplier data from the database.
/// Supports retrieval of enriched supplier data including related address information.
/// </summary>
public class SupplierRepository: BaseRepository<DAL.DTO.Supplier, Domain.Logic.Supplier>, ISupplierRepository
{
    public SupplierRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new SupplierUowMapper())
    {
    }
    
    /// <summary>
    /// Retrieves all suppliers with their associated address information.
    /// Useful for supplier listings and detail views where location is relevant.
    /// </summary>
    public async Task<IEnumerable<DAL.DTO.Supplier?>> GetEnrichedSuppliers()
    {
        var domainEntities = await RepositoryDbSet
            .Include(a => a.Address)
            .ToListAsync();

        return domainEntities.Select(e => Mapper.Map(e));
    }
}