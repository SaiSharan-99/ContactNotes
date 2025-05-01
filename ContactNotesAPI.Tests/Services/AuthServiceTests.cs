using Xunit;
using Moq;
using FluentAssertions;
using ContactNotesAPI.Services;
using ContactNotesAPI.Repositories;
using ContactNotesAPI.DTOs;
using ContactNotesAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ContactNotesAPI.Tests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task RegisterAsync_ShouldReturnToken_WhenUserIsNew()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Username = "newuser",
                Password = "securepassword"
            };

            var mockDbSet = new Mock<DbSet<User>>();
            var mockContext = new Mock<ContactNotesDbContext>();
            var userList = new List<User>().AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userList.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(userList.GetEnumerator());

            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            { "Jwt:Key", "thisisaverysecuresecretkeyusedforsigning" },
            { "Jwt:Issuer", "ContactNotesAPI" },
            { "Jwt:Audience", "ContactNotesAPIClient" }
                }).Build();

            var service = new AuthService(mockContext.Object, config);

            // Act
            var token = await service.RegisterAsync(dto);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();
            mockDbSet.Verify(d => d.Add(It.Is<User>(u => u.Username == dto.Username)), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }
        [Fact]
        public async Task RegisterAsync_ShouldReturnNull_WhenUsernameExists()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Username = "existinguser",
                Password = "somepassword"
            };

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                PasswordHash = "hashedPassword",
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            var userList = new List<User> { existingUser }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userList.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(userList.GetEnumerator());

            var mockContext = new Mock<ContactNotesDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            { "Jwt:Key", "thisisaverysecuresecretkeyusedforsigning" },
            { "Jwt:Issuer", "ContactNotesAPI" },
            { "Jwt:Audience", "ContactNotesAPIClient" }
                }).Build();

            var service = new AuthService(mockContext.Object, config);

            // Act
            var token = await service.RegisterAsync(dto);

            // Assert
            token.Should().BeNull();
            mockDbSet.Verify(d => d.Add(It.IsAny<User>()), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }
        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var password = "validpassword";
            var username = "testuser";
            var hashedPassword = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create()
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));

            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                PasswordHash = hashedPassword,
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            var userList = new List<User> { existingUser }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userList.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userList.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userList.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(userList.GetEnumerator());

            var mockContext = new Mock<ContactNotesDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            { "Jwt:Key", "thisisaverysecuresecretkeyusedforsigning" },
            { "Jwt:Issuer", "ContactNotesAPI" },
            { "Jwt:Audience", "ContactNotesAPIClient" }
                }).Build();

            var service = new AuthService(mockContext.Object, config);

            var dto = new LoginDto { Username = username, Password = password };

            // Act
            var token = await service.LoginAsync(dto);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();
        }
        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var dto = new LoginDto
            {
                Username = "nonexistentuser",
                Password = "wrongpassword"
            };

            var emptyUserList = new List<User>().AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(emptyUserList.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(emptyUserList.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(emptyUserList.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(emptyUserList.GetEnumerator());

            var mockContext = new Mock<ContactNotesDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            { "Jwt:Key", "thisisaverysecuresecretkeyusedforsigning" },
            { "Jwt:Issuer", "ContactNotesAPI" },
            { "Jwt:Audience", "ContactNotesAPIClient" }
                }).Build();

            var service = new AuthService(mockContext.Object, config);

            // Act
            var token = await service.LoginAsync(dto);

            // Assert
            token.Should().BeNull();
        }

    }
}

