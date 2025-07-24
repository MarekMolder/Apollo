using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class StorageRoomAPIMapper : IMapper<StorageRoom, BLL.DTO.StorageRoom>
{
    public StorageRoom? Map(BLL.DTO.StorageRoom? entity)
    {
        if (entity == null) return null;
        var res = new StorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            Location = entity.Location,
            EndedAt = entity.EndedAt,
        };
        return res;
    }

    public BLL.DTO.StorageRoom? Map(StorageRoom? entity)
    {
        if (entity == null) return null;
        var res = new BLL.DTO.StorageRoom
        {
            Id = entity.Id,
            Name = entity.Name,
            Location = entity.Location,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
    
    public BLL.DTO.StorageRoom Map(StorageRoomCreate entity)
    {
        var res = new BLL.DTO.StorageRoom
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            Location = entity.Location,
            EndedAt = entity.EndedAt,
        };
        return res;
    }
}