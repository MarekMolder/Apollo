using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO Address entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class AddressUowMapper: IMapper<DAL.DTO.Address, Domain.Logic.Address>
{
    // Mapper used to map related StorageRoom objects
    private readonly StorageRoomUowMapper _storageRoomUowMapper = new();
    
    // Mapper used to map related Supplier objects
    private readonly SupplierUowMapper _supplierUowMapper = new();
    
    /// <summary>
    /// Maps a full Domain Address entity to a DAL DTO Address entity, including related StorageRooms and Suppliers.
    /// </summary>
    public DAL.DTO.Address? Map(Domain.Logic.Address? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.Address
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
            StorageRooms = entity.StorageRooms?.Select(t => _storageRoomUowMapper.Map(t)).ToList()!,
            
            Suppliers = entity.Suppliers?.Select(t => _supplierUowMapper.Map(t)).ToList()!,
            
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO Address entity to a Domain Address entity, including related StorageRooms and Suppliers.
    /// </summary>
    public Domain.Logic.Address? Map(DAL.DTO.Address? entity)
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
            StorageRooms = entity.StorageRooms?.Select(t => _storageRoomUowMapper.Map(t)).ToList()!,
            
            Suppliers = entity.Suppliers?.Select(t => _supplierUowMapper.Map(t)).ToList()!,
            
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified Domain Address entity to a DAL DTO Address entity (no related StorageRooms and Suppliers).
    /// </summary>
    public static DAL.DTO.Address? MapSimple(Domain.Logic.Address? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Address
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

    /// <summary>
    /// Maps a simplified DAL DTO Address entity to a Domain Address entity (no related StorageRooms and Suppliers).
    /// </summary>
    public static Domain.Logic.Address? MapSimple(DAL.DTO.Address? entity)
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