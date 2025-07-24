using App.DAL.Contracts;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AddressRepository: BaseRepository<Address, Domain.Logic.Address>, IAddressRepository
{
    public AddressRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new AddressUOWMapper())
    {
    }
}
