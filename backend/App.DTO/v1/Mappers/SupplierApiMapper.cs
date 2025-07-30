using Base.Contracts;

namespace App.DTO.v1.Mappers;

/// <summary>
/// Maps between DTO V1 and BLL DTO Supplier objects.
/// Includes both full and simplified mapping logic.
/// </summary>
public class SupplierApiMapper : IMapper<Supplier, BLL.DTO.Supplier>
{
    /// <summary>
    /// Maps a simplified BLL DTO Supplier to DTO V1 Supplier, without related objects.
    /// </summary>
    public Supplier? Map(BLL.DTO.Supplier? entity)
    {
        if (entity == null) return null;
        var res = new Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
        return res;
    }

    /// <summary>
    /// Maps a simplified DTO V1 Supplier to BLL DTO Supplier, without related objects.
    /// </summary>
    public BLL.DTO.Supplier? Map(Supplier? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.Supplier
        {
            Id = entity.Id,
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
        return res;
    }
    
    /// <summary>
    /// Maps a simplified DTO V1 SupplierCreate to BLL DTO Supplier, without related objects.
    /// </summary>
    public BLL.DTO.Supplier Map(SupplierCreate entity)
    {
        var res = new BLL.DTO.Supplier
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            TelephoneNr = entity.TelephoneNr,
            Email = entity.Email,
            AddressId = entity.AddressId,
        };
        return res;
    }
}