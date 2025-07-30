using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO Address entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class AddressBllMapper : IMapper<BLL.DTO.Address, DAL.DTO.Address>
{
    // Mapper used to map related StorageRoom objects
    private readonly StorageRoomBllMapper _storageRoomBllMapper = new();
    
    // Mapper used to map related Supplier objects
    private readonly SupplierBllMapper _supplierBllMapper = new();
    
    /// <summary>
    /// Maps a full BLL DTO Address entity to a DAL DTO Address entity, including related StorageRooms and Suppliers.
    /// </summary>
    public DAL.DTO.Address? Map(BLL.DTO.Address? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.Address()
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
            
            StorageRooms = entity.StorageRooms?.Select(t => _storageRoomBllMapper.Map(t)).ToList()!,
            
            Suppliers = entity.Suppliers?.Select(t => _supplierBllMapper.Map(t)).ToList()!,
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO Address entity to a BLL DTO Address entity, including related StorageRooms and Suppliers.
    /// </summary>
    public BLL.DTO.Address? Map(DAL.DTO.Address? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.Address()
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
            
            StorageRooms = entity.StorageRooms?.Select(t => _storageRoomBllMapper.Map(t)).ToList()!,
            
            Suppliers = entity.Suppliers?.Select(t => _supplierBllMapper.Map(t)).ToList()!,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified BLL DTO Address entity to a DAL DTO Address entity (no related StorageRooms and Suppliers).
    /// </summary>
    public static DAL.DTO.Address? MapSimple(BLL.DTO.Address? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Address()
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
    /// Maps a simplified DAL DTO Address entity to a BLL DTO Address entity (no related StorageRooms and Suppliers).
    /// </summary>
    public static BLL.DTO.Address? MapSimple(DAL.DTO.Address? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.Address()
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
