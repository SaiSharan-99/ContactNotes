using Xunit;
using Moq;
using FluentAssertions;
using ContactNotesAPI.Services;
using ContactNotesAPI.Repositories;
using ContactNotesAPI.Models;
using ContactNotesAPI.DTOs;

namespace ContactNotesAPI.Tests.Services
{
    public class NoteServiceTests
    {
        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfNoteDtos()
        {
            // Arrange
            var fakeNotes = new List<Note>
            {
                new Note { Id = Guid.NewGuid(), ContactId = Guid.NewGuid(), Body = "Test Note 1", CreatedAt = DateTime.UtcNow },
                new Note { Id = Guid.NewGuid(), ContactId = Guid.NewGuid(), Body = "Test Note 2", CreatedAt = DateTime.UtcNow }
            };

            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(fakeNotes);

            var service = new NoteService(mockRepo.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().AllBeOfType<NoteDto>();
            result.Select(r => r.Body).Should().Contain(new[] { "Test Note 1", "Test Note 2" });
        }
        [Fact]
        public async Task AddAsync_ShouldAddNote_WhenDataIsValid()
        {
            // Arrange
            var createDto = new CreateNoteDto
            {
                ContactId = Guid.NewGuid(),
                Body = "New test note"
            };

            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Note>())).Returns(Task.CompletedTask);

            var service = new NoteService(mockRepo.Object);

            // Act
            Func<Task> act = async () => await service.AddAsync(createDto);

            // Assert
            await act.Should().NotThrowAsync();
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Note>()), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_ShouldUpdateNote_WhenNoteExists()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var existingNote = new Note
            {
                Id = noteId,
                ContactId = contactId,
                Body = "Old note body",
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            };

            var dto = new CreateNoteDto
            {
                ContactId = contactId,
                Body = "Updated note body"
            };

            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(noteId)).ReturnsAsync(existingNote);

            Note? updatedNote = null;
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Note>()))
                    .Callback<Note>(note =>
                    {
                        updatedNote = note;
                    })
                    .Returns(Task.CompletedTask);

            var service = new NoteService(mockRepo.Object);

            // Act
            await service.UpdateAsync(noteId, dto);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(noteId), Times.Once);
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Note>()), Times.Once);

            updatedNote.Should().NotBeNull();
            updatedNote!.Body.Should().Be(dto.Body);
            updatedNote.UpdatedAt.Should().HaveValue();
            updatedNote.UpdatedAt!.Value.Should().BeOnOrAfter(existingNote.UpdatedAt!.Value);
        }
        [Fact]
        public async Task UpdateAsync_ShouldNotCallUpdate_WhenNoteNotFound()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var dto = new CreateNoteDto
            {
                ContactId = Guid.NewGuid(),
                Body = "Doesn't matter"
            };

            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(noteId)).ReturnsAsync((Note?)null);

            var service = new NoteService(mockRepo.Object);

            // Act
            await service.UpdateAsync(noteId, dto);

            // Assert
            mockRepo.Verify(r => r.GetByIdAsync(noteId), Times.Once);
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Note>()), Times.Never);
        }
        [Fact]
        public async Task DeleteAsync_ShouldCallRepoDelete_WithCorrectId()
        {
            // Arrange
            var noteId = Guid.NewGuid();

            var mockRepo = new Mock<INoteRepository>();
            mockRepo.Setup(r => r.DeleteAsync(noteId)).Returns(Task.CompletedTask);

            var service = new NoteService(mockRepo.Object);

            // Act
            await service.DeleteAsync(noteId);

            // Assert
            mockRepo.Verify(r => r.DeleteAsync(noteId), Times.Once);
        }

    }
}
