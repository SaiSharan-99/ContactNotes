using Xunit;
using Moq;
using FluentAssertions;
using ContactNotesAPI.Models;
using ContactNotesAPI.Services;
using ContactNotesAPI.Repositories;
using ContactNotesAPI.DTOs;

namespace ContactNotesAPI.Tests.Services
{
    public class ContactServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfContactDtos()
        {
            // Arrange
            var fakeContacts = new List<Contact>
    {
        new Contact { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "1234567890" },
        new Contact { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "9876543210" }
    };

            var mockRepo = new Mock<IContactRepository>();
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(fakeContacts);

            var service = new ContactService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllBeOfType<ContactDto>();
            result.Select(c => c.Email).Should().Contain(new[] { "john@example.com", "jane@example.com" });
        }
        [Fact]
        public async Task AddAsync_ShouldCallRepoAdd_WithMappedContact()
        {
            // Arrange
            var dto = new CreateContactDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "1234567890"
            };

            var mockRepo = new Mock<IContactRepository>();

            Contact? savedContact = null;
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Contact>()))
                    .Callback<Contact>(c => savedContact = c)
                    .Returns(Task.CompletedTask);

            var service = new ContactService(mockRepo.Object);

            // Act
            await service.AddAsync(dto);

            // Assert
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Once);

            savedContact.Should().NotBeNull();
            savedContact!.FirstName.Should().Be(dto.FirstName);
            savedContact.LastName.Should().Be(dto.LastName);
            savedContact.Email.Should().Be(dto.Email);
            savedContact.PhoneNumber.Should().Be(dto.PhoneNumber);
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateContact_WhenContactExists()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            var existingContact = new Contact
            {
                Id = contactId,
                FirstName = "Old",
                LastName = "Name",
                Email = "old@example.com",
                PhoneNumber = "1111111111"
            };

            var dto = new CreateContactDto
            {
                FirstName = "New",
                LastName = "Name",
                Email = "new@example.com",
                PhoneNumber = "9999999999"
            };

            var mockRepo = new Mock<IContactRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(contactId)).ReturnsAsync(existingContact);
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Contact>())).Returns(Task.CompletedTask);

            var service = new ContactService(mockRepo.Object);

            // Act
            await service.UpdateAsync(contactId, dto);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(contactId), Times.Once);
            mockRepo.Verify(r => r.UpdateAsync(It.Is<Contact>(c =>
                c.Id == contactId &&
                c.FirstName == dto.FirstName &&
                c.LastName == dto.LastName &&
                c.Email == dto.Email &&
                c.PhoneNumber == dto.PhoneNumber
            )), Times.Once);
        }
        [Fact]
        public async Task DeleteAsync_ShouldCallRepoDelete_WithCorrectId()
        {
            // Arrange
            var contactId = Guid.NewGuid();

            var mockRepo = new Mock<IContactRepository>();
            mockRepo.Setup(r => r.DeleteAsync(contactId)).Returns(Task.CompletedTask);

            var service = new ContactService(mockRepo.Object);

            // Act
            await service.DeleteAsync(contactId);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(contactId), Times.Once);
        }

    }
}

