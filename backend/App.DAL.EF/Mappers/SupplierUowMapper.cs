using Base.Contracts;

namespace App.DAL.EF.Mappers;

/// <summary>
/// Maps between Domain and DAL DTO Supplier entities.
/// Handles both full mapping and simplified mapping.
/// </summary>
public class SupplierUowMapper: IMapper<DAL.DTO.Supplier, Domain.Logic.Supplier>
{
    // Mapper used to map related Supplier objects
    private readonly ActionEntityUowMapper _actionEntityUowMapper = new();
    
    // Mapper used to map related Supplier objects
    private readonly ProductUowMapper _productUowMapper = new();
    
    /// <summary>
    /// Maps a full Domain Supplier entity to a DAL DTO Supplier entity, including related Addresses and Actions.
    /// </summary>
    public DAL.DTO.Supplier? Map(Domain.Logic.Supplier? entity)
    {
        if (entity == null) return null;
        
        var res = new DAL.DTO.Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            
            AddressId = entity.AddressId,
            Address = AddressUowMapper.MapSimple(entity.Address),
            
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!,
            
            Products = entity.Products?.Select(t => _productUowMapper.Map(t)).ToList()!
        };
        return res;
    }

    /// <summary>
    /// Maps a full DAL DTO Supplier entity to a Domain Supplier entity, including related Addresses and Actions.
    /// </summary>
    public Domain.Logic.Supplier? Map(DAL.DTO.Supplier? entity)
    {
        if (entity == null) return null;
        
        var res = new Domain.Logic.Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            
            AddressId = entity.AddressId,
            Address = AddressUowMapper.MapSimple(entity.Address),
            
            Actions = entity.Actions?.Select(t => _actionEntityUowMapper.Map(t)).ToList()!,
            
            Products = entity.Products?.Select(t => _productUowMapper.Map(t)).ToList()!
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified Domain Supplier entity to a DAL DTO Supplier entity (no related Addresses and Actions).
    /// </summary>
    public static DAL.DTO.Supplier? MapSimple(Domain.Logic.Supplier? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
    }

    /// <summary>
    /// Maps a simplified DAL DTO Supplier entity to a Domain Supplier entity (no related Addresses and Actions).
    /// </summary>
    public static Domain.Logic.Supplier? MapSimple(DAL.DTO.Supplier? entity)
    {
        if (entity == null) return null;

        return new Domain.Logic.Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
    }
}
