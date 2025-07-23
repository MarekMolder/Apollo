using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using App.DTO.v1;
using App.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace App.Tests.Integration.Api;

public class HappyFlowTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HappyFlowTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task FullUserFlow_CreatesAndUpdatesActionAndStock()
    {
        // 0. Login as seeded admin
        var loginResp = await _client.PostAsJsonAsync("/api/v1/account/login", new LoginInfo
        {
            Email = "admin@example.com",
            Password = "Admin123!"
        });
        var adminJwt = await loginResp.Content.ReadFromJsonAsync<JWTResponse>();
        Assert.True(loginResp.IsSuccessStatusCode);
        Assert.NotNull(adminJwt);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminJwt!.JWT);

        // 1. Register new user (with admin rights)
        var registerResponse = await _client.PostAsJsonAsync("/api/v1/account/register", new Register
        {
            Email = "testuser@example.com",
            FirstName = "Test",
            LastName = "User",
            Password = "Password.123"
        });
        Assert.True(registerResponse.IsSuccessStatusCode);

        var jwtResponse = await registerResponse.Content.ReadFromJsonAsync<JWTResponse>();
        Assert.NotNull(jwtResponse);

        // 2. Create product category
        var category = new ProductCategory { Id = Guid.NewGuid(), Name = "Test Category" };
        var categoryResp = await _client.PostAsJsonAsync("/api/v1/productcategories", category);
        var createdCategory = await categoryResp.Content.ReadFromJsonAsync<ProductCategory>();
        Assert.True(categoryResp.IsSuccessStatusCode);
        Assert.NotNull(createdCategory);

        // 3. Create product
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Code = "TP-001",
            Price = 10,
            Unit = "pcs",
            Volume = 1,
            Quantity = 100,
            ProductCategoryId = createdCategory!.Id
        };
        var productResp = await _client.PostAsJsonAsync("/api/v1/products", product);
        var createdProduct = await productResp.Content.ReadFromJsonAsync<Product>();
        Assert.True(productResp.IsSuccessStatusCode);
        Assert.NotNull(createdProduct);

        // 4. Create storage room
        var storage = new StorageRoom { Id = Guid.NewGuid(), Name = "Main Room", Location = "3. korrus" };
        var storageResp = await _client.PostAsJsonAsync("/api/v1/storagerooms", storage);
        var createdStorage = await storageResp.Content.ReadFromJsonAsync<StorageRoom>();
        Assert.True(storageResp.IsSuccessStatusCode);
        Assert.NotNull(createdStorage);

        // 5. Create ActionType (Add + Remove)
        var actionTypeAdd = new ActionTypeEntity { Id = Guid.NewGuid(), Code = App.DTO.v1.Enums.ActionTypeEnum.Add, Name = "Add" };
        var actionTypeRemove = new ActionTypeEntity { Id = Guid.NewGuid(), Code = App.DTO.v1.Enums.ActionTypeEnum.Remove, Name = "Remove" };

        await _client.PostAsJsonAsync("/api/v1/actiontypes", actionTypeAdd);
        await _client.PostAsJsonAsync("/api/v1/actiontypes", actionTypeRemove);

        // 6. Get created ActionTypeIds
        var typesResp = await _client.GetAsync("/api/v1/actiontypes");
        var types = await typesResp.Content.ReadFromJsonAsync<List<ActionTypeEntity>>();
        var addTypeId = types!.First(x => x.Code == App.DTO.v1.Enums.ActionTypeEnum.Add).Id;
        var removeTypeId = types!.First(x => x.Code == App.DTO.v1.Enums.ActionTypeEnum.Remove).Id;
        Assert.NotEqual(Guid.Empty, addTypeId);
        Assert.NotEqual(Guid.Empty, removeTypeId);
        
        // Switch token to new user
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        // 7. Create Action1 (Add 5)
        var action1 = new ActionEntity
        {
            Id = Guid.NewGuid(),
            ProductId = createdProduct!.Id,
            StorageRoomId = createdStorage!.Id,
            Quantity = 5,
            ActionTypeId = addTypeId,
            Status = "Pending"
        };
        var actionResp1 = await _client.PostAsJsonAsync("/api/v1/actions", action1);
        var createdAction1 = await actionResp1.Content.ReadFromJsonAsync<ActionEntity>();
        Assert.True(actionResp1.IsSuccessStatusCode);
        Assert.NotNull(createdAction1);
        
        // 10. Create Action2 (Remove 3)
        var action2 = new ActionEntity
        {
            Id = Guid.NewGuid(),
            ProductId = createdProduct.Id,
            StorageRoomId = createdStorage.Id,
            Quantity = 3,
            ActionTypeId = removeTypeId,
            Status = "Pending"
        };
        var actionResp2 = await _client.PostAsJsonAsync("/api/v1/actions", action2);
        var createdAction2 = await actionResp2.Content.ReadFromJsonAsync<ActionEntity>();
        Assert.True(actionResp2.IsSuccessStatusCode);
        Assert.NotNull(createdAction2);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminJwt!.JWT);

        // 8. Accept Action1 => CurrentStock record should be created
        var acceptStatus = new StatusUpdateDto { Status = "Accepted" };
        var patchResp1 = await _client.PatchAsJsonAsync($"/api/v1/actions/{createdAction1!.Id}/status", acceptStatus);
        Assert.True(patchResp1.IsSuccessStatusCode);

        // 9. Verify that CurrentStock is created and has Quantity 5
        var currentStockResp1 = await _client.GetAsync($"/api/v1/currentstocks/bystorageroom/{createdStorage.Id}");
        var currentStocks1 = await currentStockResp1.Content.ReadFromJsonAsync<List<CurrentStock>>();
        var stock = currentStocks1!.FirstOrDefault(cs => cs.ProductId == createdProduct.Id);
        Assert.NotNull(stock);
        Assert.Equal(5, stock!.Quantity); // ✅ Confirm stock created with 5

        // 11. Accept Action2 => CurrentStock.Quantity should reduce to 2
        var patchResp2 = await _client.PatchAsJsonAsync($"/api/v1/actions/{createdAction2!.Id}/status", acceptStatus);
        Assert.True(patchResp2.IsSuccessStatusCode);

        // 12. Verify stock updated to 2
        var currentStockResp2 = await _client.GetAsync($"/api/v1/currentstocks/bystorageroom/{createdStorage.Id}");
        var currentStocks2 = await currentStockResp2.Content.ReadFromJsonAsync<List<CurrentStock>>();
        var updatedStock = currentStocks2!.FirstOrDefault(cs => cs.ProductId == createdProduct.Id);
        Assert.NotNull(updatedStock);
        Assert.Equal(2, updatedStock!.Quantity); // ✅ Confirm quantity reduced after removal

        // 13. Logout
        var logout = new LogoutInfo { RefreshToken = jwtResponse.RefreshToken };
        var logoutResp = await _client.PostAsJsonAsync("/api/v1/account/logout", logout);
        Assert.True(logoutResp.IsSuccessStatusCode);
    }
}
