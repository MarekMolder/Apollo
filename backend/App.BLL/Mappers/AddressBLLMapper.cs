using App.DAL.DTO;
using Base.Contracts;

namespace App.BLL.Mappers;

public class AddressBLLMapper : IMapper<App.BLL.DTO.Address, App.DAL.DTO.Address>
{
    private readonly InventoryBLLMapper _inventoryBllMapper = new();
    private readonly SupplierBLLMapper _supplierBllMapper = new();
    
    public Address? Map(DTO.Address? entity)
    {
        if (entity == null) return null;
        
        var res = new Address()
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
            
            Inventories = entity.Inventories?.Select(t => _inventoryBllMapper.Map(t)).ToList()!,
            
            Suppliers = entity.Suppliers?.Select(t => _supplierBllMapper.Map(t)).ToList()!,
        };
        return res;
    }

    public DTO.Address? Map(Address? entity)
    {
        if (entity == null) return null;
        
        var res = new DTO.Address()
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
            
            Inventories = entity.Inventories?.Select(t => _inventoryBllMapper.Map(t)).ToList()!,
            
            Suppliers = entity.Suppliers?.Select(t => _supplierBllMapper.Map(t)).ToList()!,
            
        };
        return res;
    }
    
    public static Address? MapSimple(DTO.Address? entity)
    {
        if (entity == null) return null;

        return new Address()
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
    
    public static DTO.Address? MapSimple(Address? entity)
    {
        if (entity == null) return null;

        return new DTO.Address()
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