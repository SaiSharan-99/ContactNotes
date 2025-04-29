using ContactNotesAPI.DTOs;
using ContactNotesAPI.Models;
using ContactNotesAPI.Repositories;

namespace ContactNotesAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repo;

        public ContactService(IContactRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ContactDto>> GetAllAsync()
        {
            var contacts = await _repo.GetAllAsync();
            return contacts.Select(c => new ContactDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber
            });
        }

        public async Task<ContactDto> GetByIdAsync(Guid id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new ContactDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber
            };
        }

        public async Task AddAsync(CreateContactDto dto)
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _repo.AddAsync(contact);
        }

        public async Task UpdateAsync(Guid id, CreateContactDto dto)
        {
            var contact = await _repo.GetByIdAsync(id);
            if (contact == null) return;

            contact.FirstName = dto.FirstName;
            contact.LastName = dto.LastName;
            contact.Email = dto.Email;
            contact.PhoneNumber = dto.PhoneNumber;
            contact.UpdatedAt = DateTime.Now;

            await _repo.UpdateAsync(contact);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
    }

}
