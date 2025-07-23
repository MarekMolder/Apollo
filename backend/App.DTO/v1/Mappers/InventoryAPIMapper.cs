using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class InventoryAPIMapper : IMapper<App.DTO.v1.Inventory, App.BLL.DTO.Inventory>
{
    public App.DTO.v1.Inventory? Map(App.BLL.DTO.Inventory? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
        return res;
    }

    public App.BLL.DTO.Inventory? Map(App.DTO.v1.Inventory? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.Inventory()
        {
            Id = entity.Id,
            Name = entity.Name,
            EndedAt = entity.EndedAt,
            AddressId = entity.AddressId,
            AllowedRoles = entity.AllowedRoles?.ToList()
        };
        return res;
    }
    
    public App.BLL.DTO.Inventory Map(App.DTO.v1.InventoryCreate entity)
    {
        var res = new App.BLL.DTO.Inventory()
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