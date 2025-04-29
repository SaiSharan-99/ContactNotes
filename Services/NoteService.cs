using ContactNotesAPI.DTOs;
using ContactNotesAPI.Models;
using ContactNotesAPI.Repositories;

namespace ContactNotesAPI.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _repo;

        public NoteService(INoteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<NoteDto>> GetAllAsync()
        {
            var notes = await _repo.GetAllAsync();
            return notes.Select(n => new NoteDto
            {
                Id = n.Id,
                ContactId = n.ContactId,
                Body = n.Body,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task<NoteDto> GetByIdAsync(Guid id)
        {
            var note = await _repo.GetByIdAsync(id);
            if (note == null) return null;
            return new NoteDto
            {
                Id = note.Id,
                ContactId = note.ContactId,
                Body = note.Body,
                CreatedAt = note.CreatedAt
            };
        }

        public async Task<IEnumerable<NoteDto>> GetByContactIdAsync(Guid contactId)
        {
            var notes = await _repo.GetByContactIdAsync(contactId);
            return notes.Select(n => new NoteDto
            {
                Id = n.Id,
                ContactId = n.ContactId,
                Body = n.Body,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task AddAsync(CreateNoteDto dto)
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                ContactId = dto.ContactId,
                Body = dto.Body,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _repo.AddAsync(note);
        }

        public async Task UpdateAsync(Guid id, CreateNoteDto dto)
        {
            var note = await _repo.GetByIdAsync(id);
            if (note == null) return;

            note.Body = dto.Body;
            note.UpdatedAt = DateTime.Now;

            await _repo.UpdateAsync(note);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
    }



}
