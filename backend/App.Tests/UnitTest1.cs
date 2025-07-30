namespace App.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_mapper_null()
    {
        // Arrange
        var mapper = new DTO.v1.Mappers.ActionEntityApiMapper();
        
        // Act
        var result = mapper.Map((App.BLL.DTO.ActionEntity?)null);
        
        // Assert
        Assert.Null(result);
    }
}