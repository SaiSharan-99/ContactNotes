using ContactNotesAPI.DTOs;

namespace ContactNotesAPI.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDto>> GetAllAsync();
        Task<NoteDto> GetByIdAsync(Guid id);
        Task<IEnumerable<NoteDto>> GetByContactIdAsync(Guid contactId);
        Task AddAsync(CreateNoteDto dto);
        Task UpdateAsync(Guid id, CreateNoteDto dto);
        Task DeleteAsync(Guid id);
    }

}
