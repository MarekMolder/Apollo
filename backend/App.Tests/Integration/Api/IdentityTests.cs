
using System.Net;
using System.Net.Http.Json;
using App.DTO.v1.Identity;
using Microsoft.AspNetCore.Mvc.Testing;

namespace App.Tests.Integration.Api;

[Collection("Sequential")]
public class IdentityTests: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;


    public IdentityTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    
    [Fact]
    public async Task Register_New_User()
    {
        // Admin login
        var adminLogin = new LoginInfo
        {
            Email = "admin@example.com",
            Password = "Admin123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/v1/account/login", adminLogin);
        loginResponse.EnsureSuccessStatusCode();

        var adminJwt = await loginResponse.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(adminJwt);

        // Set Authorization header
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminJwt!.Jwt);

        // Arrange
        var registrationData = new Register
        {
            Email = "test@test.ee",
            FirstName = "Test",
            LastName = "User",
            Password = "Password.123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/register", registrationData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(responseData);
        Assert.True(responseData.Jwt.Length > 128);
        Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
    }


    [Fact]
    public async Task Login_Existing_User()
    {
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "user@taltech.ee",
            Password = "Foo.Bar.2"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login", loginData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(responseData);
        Assert.True(responseData.Jwt.Length > 128);
        Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
    }

    [Fact]
    public async Task Login_Existing_User_Check_Rights()
    {
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "admin@example.com",
            Password = "Admin123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login", loginData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(responseData);
        Assert.True(responseData.Jwt.Length > 128);
        Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
        
        
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", responseData.Jwt);
        
        var getResponse = await _client.GetAsync("/api/v1/persons");
        getResponse.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task No_Bearer_Header_Unauthorized()
    {
        var getResponse = await _client.GetAsync("/api/v1/persons");
        Assert.Equal(HttpStatusCode.Unauthorized, getResponse.StatusCode);
    }
    
    [Fact]
    public async Task JWT_Custom_Expiration()
    {
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "admin@example.com",
            Password = "Admin123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login?jwtExpiresInSeconds=1", loginData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(responseData);
        Assert.True(responseData.Jwt.Length > 128);
        Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
        
        
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", responseData.Jwt);
        
        var getResponse = await _client.GetAsync("/api/v1/persons");
        getResponse.EnsureSuccessStatusCode();

        
        // Wait for JWT to expire
        await Task.Delay(1500);
        var getResponseAuthExpired = await _client.GetAsync("/api/v1/persons");
        
        Assert.Equal(HttpStatusCode.Unauthorized, getResponseAuthExpired.StatusCode);
    }


    [Fact]
    public async Task JWT_Refresh()
    {
        // Arrange
        var loginData = new LoginInfo()
        {
            Email = "admin@example.com",
            Password = "Admin123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/account/login?jwtExpiresInSeconds=1", loginData);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseData = await response.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(responseData);
        Assert.True(responseData.Jwt.Length > 128);
        Assert.True(responseData.RefreshToken.Length == Guid.NewGuid().ToString().Length);
        
        
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", responseData.Jwt);
        
        var getResponse = await _client.GetAsync("/api/v1/persons");
        getResponse.EnsureSuccessStatusCode();

        
        // Wait for JWT to expire
        await Task.Delay(1500);

        var getResponseAuthExpired = await _client.GetAsync("/api/v1/persons");
        
        Assert.Equal(HttpStatusCode.Unauthorized, getResponseAuthExpired.StatusCode);
        
        // Refresh JWT
        var refreshResponse = await _client.PostAsJsonAsync("/api/v1/account/RenewRefreshToken", new RefreshTokenModel()
        {
            Jwt = responseData.Jwt,
            RefreshToken = responseData.RefreshToken
        });
        
        var refreshedResponseData = await refreshResponse.Content.ReadFromJsonAsync<JwtResponse>();
        Assert.NotNull(refreshedResponseData);
        
        
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", refreshedResponseData.Jwt);
        
        var getResponse2 = await _client.GetAsync("/api/v1/persons");
        getResponse2.EnsureSuccessStatusCode();
    }
    
    
}