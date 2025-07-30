using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

/// <summary>
/// Repository implementation for accessing and querying Address data from the database.
/// </summary>
public class AddressRepository: BaseRepository<DAL.DTO.Address, Domain.Logic.Address>, IAddressRepository
{
    public AddressRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new AddressUowMapper())
    {
    }
}
