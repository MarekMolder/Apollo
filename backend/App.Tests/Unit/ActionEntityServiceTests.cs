using App.BLL.Services;
using App.DAL.Contracts;
using Base.Contracts;
using Moq;

namespace App.Tests.Unit;

public class ActionEntityServiceTests
{
    private readonly Mock<IAppUOW> _mockUow;
    private readonly Mock<IActionEntityRepository> _mockActionRepo;
    private readonly Mock<ICurrentStockRepository> _mockStockRepo;
    private readonly Mock<IMapper<App.BLL.DTO.ActionEntity, App.DAL.DTO.ActionEntity>> _mockMapperBll;
    private readonly Mock<IMapper<App.DAL.DTO.ActionEntity, Domain.Logic.ActionEntity>> _mockMapperDomain;
    private readonly Mock<IMapper<App.DAL.DTO.CurrentStock, Domain.Logic.CurrentStock>> _mockStockMapper;

    private readonly ActionEntityService _service;

    public ActionEntityServiceTests()
    {
        _mockUow = new Mock<IAppUOW>();
        _mockActionRepo = new Mock<IActionEntityRepository>();
        _mockStockRepo = new Mock<ICurrentStockRepository>();
        _mockMapperBll = new Mock<IMapper<App.BLL.DTO.ActionEntity, App.DAL.DTO.ActionEntity>>();
        _mockMapperDomain = new Mock<IMapper<App.DAL.DTO.ActionEntity, Domain.Logic.ActionEntity>>();
        _mockStockMapper = new Mock<IMapper<App.DAL.DTO.CurrentStock, Domain.Logic.CurrentStock>>();

        _mockUow.SetupGet(x => x.ActionEntityRepository).Returns(_mockActionRepo.Object);
        _mockUow.SetupGet(x => x.CurrentStockRepository).Returns(_mockStockRepo.Object);

        _service = new ActionEntityService(
            _mockUow.Object,
            _mockMapperBll.Object,
            _mockStockMapper.Object,
            _mockMapperDomain.Object
        );
    }

    [Fact]
    public async Task UpdateStatusAsync_ActionNotFound_ReturnsFalse()
    {
        // Arrange
        var fakeId = Guid.NewGuid();
        _mockActionRepo.Setup(r => r.FindAsync(fakeId)).ReturnsAsync((Domain.Logic.ActionEntity?)null);

        // Act
        var result = await _service.UpdateStatusAsync(fakeId, "Accepted");

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task UpdateStatusAsync_InvalidStatus_ThrowsArgumentException()
    {
        // Arrange
        var fakeId = Guid.NewGuid();
        var fakeAction = new Domain.Logic.ActionEntity { Id = fakeId };

        _mockActionRepo.Setup(r => r.FindAsync(fakeId)).ReturnsAsync(fakeAction);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.UpdateStatusAsync(fakeId, "Pending")); // <- kehtetu staatus
    }

    [Fact]
    public async Task UpdateStatusAsync_AcceptedStatus_MakesNewCurrentStock()
    {
        // Arrange
        var fakeId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var storageId = Guid.NewGuid();

        var actionType = new App.BLL.DTO.ActionTypeEntity()
        {
            Code = BLL.DTO.Enums.ActionTypeEnum.Add
        };

        var bllAction = new App.BLL.DTO.ActionEntity
        {
            Id = fakeId,
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 3,
            ActionType = actionType
        };

        var dalAction = new App.DAL.DTO.ActionEntity();

        var fakeAction = new Domain.Logic.ActionEntity
        {
            Id = fakeId,
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 3,
            Status = "Pending",
            ActionTypeId = actionType.Id
        };

        _mockActionRepo.Setup(r => r.FindAsync(fakeId)).ReturnsAsync(fakeAction);
        _mockMapperDomain.Setup(m => m.Map(fakeAction)).Returns(dalAction);
        _mockMapperBll.Setup(m => m.Map(dalAction)).Returns(bllAction);

        _mockStockRepo.Setup(r => r.FindByProductAndStorageAsync(productId, storageId)).ReturnsAsync((Domain.Logic.CurrentStock?)null);

        App.DAL.DTO.CurrentStock? createdStock = null;
        _mockStockRepo
            .Setup(r => r.AddAsync(It.IsAny<App.DAL.DTO.CurrentStock>(), It.IsAny<Guid>()))
            .Callback<App.DAL.DTO.CurrentStock, Guid?>((s, _) => createdStock = s)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateStatusAsync(fakeId, "Accepted");

        // Assert
        Assert.True(result);
        Assert.NotNull(createdStock);
        Assert.Equal(3, createdStock!.Quantity);
        Assert.Equal(productId, createdStock.ProductId);
        Assert.Equal(storageId, createdStock.StorageRoomId);
    }
    
    [Fact]
    public async Task UpdateStatusAsync_AcceptedStatus_RemovesFromExistingStock()
    {
        // Arrange
        var fakeId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var storageId = Guid.NewGuid();

        var actionType = new App.BLL.DTO.ActionTypeEntity()
        {
            Code = BLL.DTO.Enums.ActionTypeEnum.Remove
        };

        var bllAction = new App.BLL.DTO.ActionEntity
        {
            Id = fakeId,
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 2,
            ActionType = actionType
        };

        var dalAction = new App.DAL.DTO.ActionEntity();

        var fakeAction = new Domain.Logic.ActionEntity
        {
            Id = fakeId,
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 2,
            Status = "Pending",
            ActionTypeId = actionType.Id
        };

        _mockActionRepo.Setup(r => r.FindAsync(fakeId)).ReturnsAsync(fakeAction);
        _mockMapperDomain.Setup(m => m.Map(fakeAction)).Returns(dalAction);
        _mockMapperBll.Setup(m => m.Map(dalAction)).Returns(bllAction);
        
        var currentStock = new Domain.Logic.CurrentStock
        {
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 10
        };

        var dalStock = new App.DAL.DTO.CurrentStock
        {
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 10
        };

        _mockStockRepo.Setup(r => r.FindByProductAndStorageAsync(productId, storageId))
            .ReturnsAsync(currentStock);

        _mockStockMapper.Setup(m => m.Map(currentStock)).Returns(dalStock);

        App.DAL.DTO.CurrentStock? updatedStock = null;
        _mockStockRepo
            .Setup(r => r.UpdateAsync(It.IsAny<App.DAL.DTO.CurrentStock>(), It.IsAny<Guid>()))
            .Callback<App.DAL.DTO.CurrentStock, Guid?>((s, _) => updatedStock = s)
            .ReturnsAsync(dalStock); 

        // Act
        var result = await _service.UpdateStatusAsync(fakeId, "Accepted");

        // Assert
        Assert.True(result);
        Assert.NotNull(updatedStock);
        Assert.Equal(8, updatedStock!.Quantity); // 10 - 2
        Assert.Equal(productId, updatedStock.ProductId);
        Assert.Equal(storageId, updatedStock.StorageRoomId);
    }
    
    [Fact]
    public async Task UpdateStatusAsync_DeclinedStatus_DoesNotChangeStock()
    {
        // Arrange
        var fakeId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var storageId = Guid.NewGuid();

        var actionType = new App.BLL.DTO.ActionTypeEntity()
        {
            Code = BLL.DTO.Enums.ActionTypeEnum.Add
        };

        var bllAction = new App.BLL.DTO.ActionEntity
        {
            Id = fakeId,
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 5,
            ActionType = actionType
        };

        var dalAction = new App.DAL.DTO.ActionEntity();

        var fakeAction = new Domain.Logic.ActionEntity
        {
            Id = fakeId,
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 5,
            Status = "Pending",
            ActionTypeId = actionType.Id
        };

        _mockActionRepo.Setup(r => r.FindAsync(fakeId)).ReturnsAsync(fakeAction);
        _mockMapperDomain.Setup(m => m.Map(fakeAction)).Returns(dalAction);
        _mockMapperBll.Setup(m => m.Map(dalAction)).Returns(bllAction);

        // Act
        var result = await _service.UpdateStatusAsync(fakeId, "Declined");

        // Assert
        Assert.True(result);
        
        _mockStockRepo.Verify(r => r.AddAsync(It.IsAny<App.DAL.DTO.CurrentStock>(), It.IsAny<Guid>()), Times.Never);
        _mockStockRepo.Verify(r => r.UpdateAsync(It.IsAny<App.DAL.DTO.CurrentStock>(), It.IsAny<Guid>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateStatusAsync_UnknownActionType_ThrowsInvalidOperationException()
    {
        // Arrange
        var fakeId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var storageId = Guid.NewGuid();

        var actionType = new App.BLL.DTO.ActionTypeEntity()
        {
            Code = (App.BLL.DTO.Enums.ActionTypeEnum)999
        };

        var bllAction = new App.BLL.DTO.ActionEntity
        {
            Id = fakeId,
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 2,
            ActionType = actionType
        };

        var dalAction = new App.DAL.DTO.ActionEntity();

        var fakeAction = new Domain.Logic.ActionEntity
        {
            Id = fakeId,
            ProductId = productId,
            StorageRoomId = storageId,
            Quantity = 2,
            Status = "Pending",
            ActionTypeId = actionType.Id
        };

        _mockActionRepo.Setup(r => r.FindAsync(fakeId)).ReturnsAsync(fakeAction);
        _mockMapperDomain.Setup(m => m.Map(fakeAction)).Returns(dalAction);
        _mockMapperBll.Setup(m => m.Map(dalAction)).Returns(bllAction);

        _mockStockRepo.Setup(r => r.FindByProductAndStorageAsync(productId, storageId))
            .ReturnsAsync(new Domain.Logic.CurrentStock());

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateStatusAsync(fakeId, "Accepted"));
    }
    
    [Fact]
    public async Task GetEnrichedActionEntities_ReturnsMappedResults()
    {
        // Arrange
        var dalList = new List<App.DAL.DTO.ActionEntity>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };

        var bllList = new List<App.BLL.DTO.ActionEntity>
        {
            new() { Id = dalList[0].Id },
            new() { Id = dalList[1].Id }
        };

        // Mock repository tagastus
        _mockActionRepo.Setup(r => r.GetEnrichedActionEntities()).ReturnsAsync(dalList);

        // Mock mappimine igale elemendile
        _mockMapperBll.Setup(m => m.Map(dalList[0])).Returns(bllList[0]);
        _mockMapperBll.Setup(m => m.Map(dalList[1])).Returns(bllList[1]);

        // Act
        var result = (await _service.GetEnrichedActionEntities()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(bllList[0].Id, result[0]!.Id);
        Assert.Equal(bllList[1].Id, result[1]!.Id);
    }
    
    [Fact]
    public async Task GetTopRemovedProductsAsync_ReturnsExpectedData()
    {
        // Arrange
        var expected = new List<(Guid ProductId, string ProductName, decimal RemoveQuantity)>
        {
            (Guid.NewGuid(), "Milk", 5),
            (Guid.NewGuid(), "Bread", 3)
        };

        _mockActionRepo.Setup(r => r.GetTopRemovedProductsAsync()).ReturnsAsync(expected);

        // Act
        var result = (await _service.GetTopRemovedProductsAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Milk", result[0].ProductName);
        Assert.Equal(5, result[0].RemoveQuantity);
    }
    
    [Fact]
    public async Task GetTopUsersByRemovedQuantityAsync_ReturnsExpectedData()
    {
        // Arrange
        var expected = new List<(string CreatedBy, decimal TotalRemovedQuantity)>
        {
            ("alice@example.com", 10),
            ("bob@example.com", 7)
        };

        _mockActionRepo.Setup(r => r.GetTopUsersByRemovedQuantityAsync()).ReturnsAsync(expected);

        // Act
        var result = (await _service.GetTopUsersByRemovedQuantityAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("alice@example.com", result[0].CreatedBy);
        Assert.Equal(10, result[0].TotalRemovedQuantity);
    }
}