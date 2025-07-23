using App.DAL.Contracts;
using App.DAL.EF.Mappers;
using App.Domain.Logic;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AddressRepository: BaseRepository<App.DAL.DTO.Address, App.Domain.Logic.Address>, IAddressRepository
{
    public AddressRepository(DbContext repositoryDbContext) : base(repositoryDbContext, new AddressUOWMapper())
    {
    }
}
