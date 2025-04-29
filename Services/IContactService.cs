using ContactNotesAPI.DTOs;

namespace ContactNotesAPI.Services
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> GetAllAsync();
        Task<ContactDto> GetByIdAsync(Guid id);
        Task AddAsync(CreateContactDto dto);
        Task UpdateAsync(Guid id, CreateContactDto dto);
        Task DeleteAsync(Guid id);
    }

}
