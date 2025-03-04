using Moq;
using OrderService.Controllers;
using OrderService.Data;
using OrderService.Models;
using OrderService.Models.Requests;
using OrderService.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Config;

namespace OrderService.Tests.Controllers
{
    public class AuthControllerTests : TestBase
    {
        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var context = ServiceProvider.GetRequiredService<ErpDbContext>();
            var user = new User { Id = 1, Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Email = "admin@erp.com" };
            context.Users.Add(user);
            context.SaveChanges();

            var controller = new AuthController(context, ServiceProvider.GetRequiredService<Utilities>());
            var loginRequest = new LoginRequest { Username = "admin", Password = "wrongpassword" };

            // Act
            var result = controller.Login(loginRequest);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var context = ServiceProvider.GetRequiredService<ErpDbContext>();
            var controller = new AuthController(context, ServiceProvider.GetRequiredService<Utilities>());
            var registerRequest = new RegisterRequest { Username = "newuser", Password = "password123", Email = "newuser@erp.com" };

            // Act
            var result = controller.Register(registerRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully.", okResult.Value);
        }

        [Fact]
        public void Register_ExistingUsernameOrEmail_ReturnsBadRequest()
        {
            // Arrange
            var context = ServiceProvider.GetRequiredService<ErpDbContext>();
            var user = new User { Id = 1, Username = "testuser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Email = "testuser@erp.com" };
            context.Users.Add(user);
            context.SaveChanges();

            var controller = new AuthController(context, ServiceProvider.GetRequiredService<Utilities>());
            var registerRequest = new RegisterRequest { Username = "testuser", Password = "newpass123", Email = "newemail@erp.com" };

            // Act
            var result = controller.Register(registerRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Username or email already exists.", badRequestResult.Value);
        }
    }
}