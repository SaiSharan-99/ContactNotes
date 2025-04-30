using ContactNotesAPI.DTOs;
using ContactNotesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactNotesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactsController(IContactService service) : ControllerBase
    {
        private readonly IContactService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _service.GetAllAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var contact = await _service.GetByIdAsync(id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateContactDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetAll), null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateContactDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

}
