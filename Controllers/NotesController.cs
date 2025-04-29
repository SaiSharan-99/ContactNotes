using ContactNotesAPI.DTOs;
using ContactNotesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactNotesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _service;

        public NotesController(INoteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var notes = await _service.GetAllAsync();
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var note = await _service.GetByIdAsync(id);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpGet("contact/{contactId}")]
        public async Task<IActionResult> GetByContactId(Guid contactId)
        {
            var notes = await _service.GetByContactIdAsync(contactId);
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNoteDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetAll), null);
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateNoteDto dto)
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
