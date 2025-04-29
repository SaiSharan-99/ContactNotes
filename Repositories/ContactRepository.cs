using ContactNotesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactNotesAPI.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactNotesDbContext _context;

        public ContactRepository(ContactNotesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
            => await _context.Contacts.ToListAsync();

        public async Task<Contact> GetByIdAsync(Guid id)
            => await _context.Contacts.FindAsync(id);

        public async Task AddAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }
    }


}
