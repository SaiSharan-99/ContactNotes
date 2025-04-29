using ContactNotesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactNotesAPI.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ContactNotesDbContext _context;

        public NoteRepository(ContactNotesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Note>> GetAllAsync()
            => await _context.Notes.ToListAsync();

        public async Task<Note> GetByIdAsync(Guid id)
            => await _context.Notes.FindAsync(id);

        public async Task<IEnumerable<Note>> GetByContactIdAsync(Guid contactId)
            => await _context.Notes.Where(n => n.ContactId == contactId).ToListAsync();

        public async Task AddAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }
    }


}
