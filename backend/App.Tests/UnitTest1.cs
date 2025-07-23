namespace App.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_mapper_null()
    {
        // Arrange
        var mapper = new App.DTO.v1.Mappers.ActionEntityAPIMapper();
        
        // Act
        var result = mapper.Map((App.BLL.DTO.ActionEntity?)null);
        
        // Assert
        Assert.Null(result);
    }
}