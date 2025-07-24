using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class InventoryAPIMapper : IMapper<Inventory, BLL.DTO.Inventory>
{
    public Inventory? Map(BLL.DTO.Inventory? entity)
    {
        if (entity == null) return null;
        var res = new Inventory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
        return res;
    }

    public BLL.DTO.Inventory? Map(Inventory? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.Inventory
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
        return res;
    }
    
    public BLL.DTO.Inventory Map(InventoryCreate entity)
    {
        var res = new BLL.DTO.Inventory
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
        return res;
    }
}