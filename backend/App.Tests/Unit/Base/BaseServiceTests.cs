using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.BLL;
using Base.Contracts;
using Base.DAL.Contracts;
using Moq;
using Xunit;

namespace App.Tests.Unit.Base;

public class BllTestEntity : IDomainId<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class DalTestEntity : IDomainId<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class BaseServiceTests
{
    private readonly Mock<IMapper<BllTestEntity, DalTestEntity, Guid>> _mockMapper;
    private readonly Mock<IBaseRepository<DalTestEntity, Guid>> _mockRepo;
    private readonly Mock<IBaseUOW> _mockUow;

    private readonly BaseService<BllTestEntity, DalTestEntity, IBaseRepository<DalTestEntity, Guid>, Guid> _service;

    public BaseServiceTests()
    {
        _mockRepo = new Mock<IBaseRepository<DalTestEntity, Guid>>();
        _mockMapper = new Mock<IMapper<BllTestEntity, DalTestEntity, Guid>>();
        _mockUow = new Mock<IBaseUOW>();

        _service = new BaseService<BllTestEntity, DalTestEntity, IBaseRepository<DalTestEntity, Guid>, Guid>(
            _mockUow.Object,
            _mockRepo.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public void All_ReturnsMappedEntities()
    {
        var dal = new DalTestEntity { Id = Guid.NewGuid(), Name = "Test" };
        var bll = new BllTestEntity { Id = dal.Id, Name = dal.Name };

        _mockRepo.Setup(r => r.All(It.IsAny<Guid>())).Returns(new List<DalTestEntity> { dal });
        _mockMapper.Setup(m => m.Map(dal)).Returns(bll);

        var result = _service.All().ToList();

        Assert.Single(result);
        Assert.Equal("Test", result[0].Name);
    }

    [Fact]
    public async Task AllAsync_ReturnsMappedEntities()
    {
        var dal = new DalTestEntity { Id = Guid.NewGuid(), Name = "AsyncTest" };
        var bll = new BllTestEntity { Id = dal.Id, Name = dal.Name };

        _mockRepo.Setup(r => r.AllAsync(It.IsAny<Guid>())).ReturnsAsync(new List<DalTestEntity> { dal });
        _mockMapper.Setup(m => m.Map(dal)).Returns(bll);

        var result = (await _service.AllAsync()).ToList();

        Assert.Single(result);
        Assert.Equal("AsyncTest", result[0].Name);
    }

    [Fact]
    public void Find_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();
        var dal = new DalTestEntity { Id = id, Name = "Find" };
        var bll = new BllTestEntity { Id = id, Name = dal.Name };

        _mockRepo.Setup(r => r.Find(id, It.IsAny<Guid>())).Returns(dal);
        _mockMapper.Setup(m => m.Map(dal)).Returns(bll);

        var result = _service.Find(id);

        Assert.NotNull(result);
        Assert.Equal("Find", result!.Name);
    }

    [Fact]
    public async Task FindAsync_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();
        var dal = new DalTestEntity { Id = id, Name = "FindAsync" };
        var bll = new BllTestEntity { Id = id, Name = dal.Name };

        _mockRepo.Setup(r => r.FindAsync(id, It.IsAny<Guid>())).ReturnsAsync(dal);
        _mockMapper.Setup(m => m.Map(dal)).Returns(bll);

        var result = await _service.FindAsync(id);

        Assert.NotNull(result);
        Assert.Equal("FindAsync", result!.Name);
    }

    [Fact]
    public void Add_CallsRepository()
    {
        var bll = new BllTestEntity { Id = Guid.NewGuid(), Name = "Add" };
        var dal = new DalTestEntity { Id = bll.Id, Name = bll.Name };

        _mockMapper.Setup(m => m.Map(bll)).Returns(dal);
        _mockRepo.Setup(r => r.Add(dal, It.IsAny<Guid>())).Verifiable();

        _service.Add(bll);

        _mockRepo.Verify();
    }

    [Fact]
    public async Task AddAsync_CallsRepository()
    {
        var bll = new BllTestEntity { Id = Guid.NewGuid(), Name = "AddAsync" };
        var dal = new DalTestEntity { Id = bll.Id, Name = bll.Name };

        _mockMapper.Setup(m => m.Map(bll)).Returns(dal);
        _mockRepo.Setup(r => r.AddAsync(dal, It.IsAny<Guid>())).Returns(Task.CompletedTask).Verifiable();

        await _service.AddAsync(bll);

        _mockRepo.Verify();
    }

    [Fact]
    public void Update_CallsRepositoryAndReturnsMapped()
    {
        var bll = new BllTestEntity { Id = Guid.NewGuid(), Name = "Update" };
        var dal = new DalTestEntity { Id = bll.Id, Name = bll.Name };

        _mockMapper.Setup(m => m.Map(bll)).Returns(dal);
        _mockRepo.Setup(r => r.Update(dal, It.IsAny<Guid>())).Returns(dal);
        _mockMapper.Setup(m => m.Map(dal)).Returns(bll);

        var result = _service.Update(bll);

        Assert.NotNull(result);
        Assert.Equal("Update", result!.Name);
    }

    [Fact]
    public async Task UpdateAsync_CallsRepositoryAndReturnsMapped()
    {
        var bll = new BllTestEntity { Id = Guid.NewGuid(), Name = "UpdateAsync" };
        var dal = new DalTestEntity { Id = bll.Id, Name = bll.Name };

        _mockMapper.Setup(m => m.Map(bll)).Returns(dal);
        _mockRepo.Setup(r => r.UpdateAsync(dal, It.IsAny<Guid>())).ReturnsAsync(dal);
        _mockMapper.Setup(m => m.Map(dal)).Returns(bll);

        var result = await _service.UpdateAsync(bll);

        Assert.NotNull(result);
        Assert.Equal("UpdateAsync", result!.Name);
    }

    [Fact]
    public void Remove_ById_CallsRepository()
    {
        var id = Guid.NewGuid();
        var dal = new DalTestEntity { Id = id };

        _mockRepo.Setup(r => r.Find(id, It.IsAny<Guid>())).Returns(dal);
        _mockRepo.Setup(r => r.Remove(dal, It.IsAny<Guid>())).Verifiable();

        _service.Remove(id);

        _mockRepo.Verify();
    }

    [Fact]
    public async Task RemoveAsync_CallsRepository()
    {
        var id = Guid.NewGuid();
        var dal = new DalTestEntity { Id = id };

        _mockRepo.Setup(r => r.FindAsync(id, It.IsAny<Guid>())).ReturnsAsync(dal);
        _mockRepo.Setup(r => r.RemoveAsync(id, It.IsAny<Guid>())).Returns(Task.CompletedTask).Verifiable();

        await _service.RemoveAsync(id);

        _mockRepo.Verify();
    }

    [Fact]
    public void Exists_ReturnsTrue_IfEntityFound()
    {
        var id = Guid.NewGuid();
        var dal = new DalTestEntity { Id = id };

        _mockRepo.Setup(r => r.Find(id, It.IsAny<Guid>())).Returns(dal);

        var result = _service.Exists(id);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsTrue_IfEntityFound()
    {
        var id = Guid.NewGuid();
        var dal = new DalTestEntity { Id = id };

        _mockRepo.Setup(r => r.FindAsync(id, It.IsAny<Guid>())).ReturnsAsync(dal);

        var result = await _service.ExistsAsync(id);

        Assert.True(result);
    }

    [Fact]
    public void Remove_ByEntity_CallsRemoveById()
    {
        var id = Guid.NewGuid();
        var bll = new BllTestEntity { Id = id };

        var service = new Mock<BaseService<BllTestEntity, DalTestEntity, IBaseRepository<DalTestEntity, Guid>, Guid>>(
            _mockUow.Object,
            _mockRepo.Object,
            _mockMapper.Object
        ) { CallBase = true };

        var called = false;
        service.Setup(s => s.Remove(id, It.IsAny<Guid>())).Callback(() => called = true);

        service.Object.Remove(bll);

        Assert.True(called);
    }
}
