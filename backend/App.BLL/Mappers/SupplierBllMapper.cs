using Base.Contracts;

namespace App.BLL.Mappers;

/// <summary>
/// Maps between BLL DTO and DAL DTO Supplier entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class SupplierBllMapper : IMapper<BLL.DTO.Supplier, DAL.DTO.Supplier>
{
    // Mapper used to map related Supplier objects
    private readonly ActionEntityBllMapper _actionEntityBllMapper = new();
    
    /// <summary>
    /// Maps a full BLL DTO Supplier entity to a DAL DTO Supplier entity, including related Addresses and Actions.
    /// </summary>
    public DAL.DTO.Supplier? Map(BLL.DTO.Supplier? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            
            AddressId = entity.AddressId,
            Address = AddressBllMapper.MapSimple(entity.Address),
            
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO Supplier entity to a BLL DTO Supplier entity, including related Addresses and Actions.
    /// </summary>
    public BLL.DTO.Supplier? Map(DAL.DTO.Supplier? entity)
    {
        if (entity == null) return null;
        
        var res = new BLL.DTO.Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            
            AddressId = entity.AddressId,
            Address = AddressBllMapper.MapSimple(entity.Address),
            
            Actions = entity.Actions?.Select(t => _actionEntityBllMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified BLL DTO Supplier entity to a DAL DTO Supplier entity (no related Addresses and Actions).
    /// </summary>
    public static DAL.DTO.Supplier? MapSimple(BLL.DTO.Supplier? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
    }
    
    /// <summary>
    /// Maps a simplified DAL DTO Supplier entity to a BLL DTO Supplier entity (no related Addresses and Actions).
    /// </summary>
    public static BLL.DTO.Supplier? MapSimple(DAL.DTO.Supplier? entity)
    {
        if (entity == null) return null;

        return new BLL.DTO.Supplier()
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
    }
}
