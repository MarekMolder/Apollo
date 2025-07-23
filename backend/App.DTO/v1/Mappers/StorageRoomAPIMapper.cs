using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class StorageRoomAPIMapper : IMapper<App.DTO.v1.StorageRoom, App.BLL.DTO.StorageRoom>
{
    public App.DTO.v1.StorageRoom? Map(App.BLL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            Location = entity.Location,
            EndedAt = entity.EndedAt,
        };
        return res;
    }

    public App.BLL.DTO.StorageRoom? Map(App.DTO.v1.StorageRoom? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.StorageRoom()
        {
            Id = entity.Id,
            Name = entity.Name,
            Location = entity.Location,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
    
    public App.BLL.DTO.StorageRoom Map(App.DTO.v1.StorageRoomCreate entity)
    {
        var res = new App.BLL.DTO.StorageRoom()
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            Location = entity.Location,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
}