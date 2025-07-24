using App.DAL.DTO;
using Base.Contracts;

namespace App.DAL.EF.Mappers;

public class AddressUOWMapper: IMapper<Address, Domain.Logic.Address>
{
    private readonly InventoryUOWMapper _inventoryUOWMapper = new();
    private readonly SupplierUOWMapper _supplierUOWMapper = new();
    public Address? Map(Domain.Logic.Address? entity)
    {
        if (entity == null) return null;
        
        var res = new Address
        {
            Id = entity.Id,
            StreetName = entity.StreetName,
            BuildingNr = entity.BuildingNr,
            PostalCode = entity.PostalCode,
            City = entity.City,
            Province = entity.Province,
            Country = entity.Country,
            Name = entity.Name,
            UnitNr = entity.UnitNr,
            Inventories = entity.Inventories?.Select(t => _inventoryUOWMapper.Map(t)).ToList()!,
            
            Suppliers = entity.Suppliers?.Select(t => _supplierUOWMapper.Map(t)).ToList()!,
            
        };
        return res;
    }

    public Domain.Logic.Address? Map(Address? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.Address
        {
            Id = entity.Id,
            StreetName = entity.StreetName,
            BuildingNr = entity.BuildingNr,
            PostalCode = entity.PostalCode,
            City = entity.City,
            Province = entity.Province,
            Country = entity.Country,
            Name = entity.Name,
            UnitNr = entity.UnitNr,
            Inventories = entity.Inventories?.Select(t => _inventoryUOWMapper.Map(t)).ToList()!,
            
            Suppliers = entity.Suppliers?.Select(t => _supplierUOWMapper.Map(t)).ToList()!,
            
        };
        return res;
    }
    
    public static Address? MapSimple(Domain.Logic.Address? entity)
    {
        if (entity == null) return null;

        return new Address
        {
            Id = entity.Id,
            StreetName = entity.StreetName,
            BuildingNr = entity.BuildingNr,
            PostalCode = entity.PostalCode,
            City = entity.City,
            Province = entity.Province,
            Country = entity.Country,
            Name = entity.Name,
            UnitNr = entity.UnitNr,
        };
    }

    public static Domain.Logic.Address? MapSimple(Address? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.Address
        {
            Id = entity.Id,
            StreetName = entity.StreetName,
            BuildingNr = entity.BuildingNr,
            PostalCode = entity.PostalCode,
            City = entity.City,
            Province = entity.Province,
            Country = entity.Country,
            Name = entity.Name,
            UnitNr = entity.UnitNr,
        };
    }
}