using LokatyWebApi.Controllers;
using LokatyWebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class ClientControllerTests
{
    [Fact]
    public async Task GetClients_ReturnsListOfClients()
    {
        // Arrange
        var mockDbContext = new Mock<LokatyContext>();
        var clients = new List<Client>
        {
            new Client { ClientId = 1, FirstName = "John", LastName = "Doe" },
            new Client { ClientId = 2, FirstName = "Jane", LastName = "Doe" }
        }.AsQueryable();

        var mockDbSet = new Mock<DbSet<Client>>();
        mockDbSet.As<IQueryable<Client>>().Setup(m => m.Provider).Returns(clients.Provider);
        mockDbSet.As<IQueryable<Client>>().Setup(m => m.Expression).Returns(clients.Expression);
        mockDbSet.As<IQueryable<Client>>().Setup(m => m.ElementType).Returns(clients.ElementType);
        mockDbSet.As<IQueryable<Client>>().Setup(m => m.GetEnumerator()).Returns(clients.GetEnumerator());

        mockDbContext.Setup(m => m.Clients).Returns(mockDbSet.Object);

        var controller = new ClientController(mockDbContext.Object);

        // Act
        var result = await controller.GetClients();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Client>>(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AddClient_ReturnsAddedClient()
    {
        // Arrange
        var mockDbContext = new Mock<LokatyContext>();
        var controller = new ClientController(mockDbContext.Object);

        var newClient = new Client { ClientId = 1, FirstName = "John", LastName = "Doe" };

        // Act
        var result = await controller.AddClient(newClient);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Client>(result);
        Assert.Equal(newClient.ClientId, result.ClientId);
    }

    [Fact]
    public async Task UpdateClient_ReturnsOkResult()
    {
        // Arrange
        var mockDbContext = new Mock<LokatyContext>();
        var controller = new ClientController(mockDbContext.Object);

        var existingClient = new Client { ClientId = 1, FirstName = "John", LastName = "Doe" };

        mockDbContext.Setup(m => m.Clients.FindAsync(existingClient.ClientId)).ReturnsAsync(existingClient);

        // Act
        var result = await controller.UpdateClient(existingClient.ClientId, existingClient);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void DeleteClient_ReturnsTrueIfClientExists()
    {
        // Arrange
        var mockDbContext = new Mock<LokatyContext>();
        var controller = new ClientController(mockDbContext.Object);

        var existingClient = new Client { ClientId = 1, FirstName = "John", LastName = "Doe" };

        mockDbContext.Setup(m => m.Clients.Find(existingClient.ClientId)).Returns(existingClient);

        // Act
        var result = controller.DeleteClient(existingClient.ClientId);

        // Assert
        Assert.True(result);
    }
}
