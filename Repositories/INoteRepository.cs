using ContactNotesAPI.Models;

namespace ContactNotesAPI.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllAsync();
        Task<Note> GetByIdAsync(Guid id);
        Task<IEnumerable<Note>> GetByContactIdAsync(Guid contactId);
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(Guid id);
    }

}
