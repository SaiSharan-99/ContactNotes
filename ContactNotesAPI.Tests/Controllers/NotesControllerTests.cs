using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ContactNotesAPI.Controllers;
using ContactNotesAPI.DTOs;
using ContactNotesAPI.Services;

namespace ContactNotesAPI.Tests.Controllers
{
    public class NotesControllerTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnOkWithNotes()
        {
            // Arrange
            var notes = new List<NoteDto>
            {
                new NoteDto { Id = Guid.NewGuid(), ContactId = Guid.NewGuid(), Body = "Note 1", CreatedAt = DateTime.UtcNow },
                new NoteDto { Id = Guid.NewGuid(), ContactId = Guid.NewGuid(), Body = "Note 2", CreatedAt = DateTime.UtcNow }
            };

            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(notes);

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(notes);
        }
        [Fact]
        public async Task Create_ShouldReturnCreatedResult_WhenNoteIsAdded()
        {
            // Arrange
            var dto = new CreateNoteDto
            {
                ContactId = Guid.NewGuid(),
                Body = "New note"
            };

            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.AddAsync(dto)).Returns(Task.CompletedTask);

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.Create(dto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.StatusCode.Should().Be(201);
        }
        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenNoteIsUpdated()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var dto = new CreateNoteDto
            {
                ContactId = Guid.NewGuid(),
                Body = "Updated note"
            };

            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.UpdateAsync(noteId, dto)).Returns(Task.CompletedTask);

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.Update(noteId, dto);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }
        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenNoteIsDeleted()
        {
            // Arrange
            var noteId = Guid.NewGuid();

            var mockService = new Mock<INoteService>();
            mockService.Setup(s => s.DeleteAsync(noteId)).Returns(Task.CompletedTask);

            var controller = new NotesController(mockService.Object);

            // Act
            var result = await controller.Delete(noteId);

            // Assert
            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }

    }
}

