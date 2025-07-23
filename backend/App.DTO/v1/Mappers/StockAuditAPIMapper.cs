using Base.Contracts;

namespace App.DTO.v1.Mappers;

public class StockAuditAPIMapper : IMapper<App.DTO.v1.StockAudit, App.BLL.DTO.StockAudit>
{
    public App.DTO.v1.StockAudit? Map(App.BLL.DTO.StockAudit? entity)
    {
        if (entity == null) return null;
        var res = new App.DTO.v1.StockAudit()
        {
            Id = entity.Id,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }

    public App.BLL.DTO.StockAudit? Map(App.DTO.v1.StockAudit? entity)
    {
        if (entity == null) return null;
        var res = new App.BLL.DTO.StockAudit()
        {
            Id = entity.Id,
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
    
    public App.BLL.DTO.StockAudit Map(App.DTO.v1.StockAuditCreate entity)
    {
        var res = new App.BLL.DTO.StockAudit()
        {
            Id = Guid.NewGuid(),
            StorageRoomId = entity.StorageRoomId,
        };
        return res;
    }
}