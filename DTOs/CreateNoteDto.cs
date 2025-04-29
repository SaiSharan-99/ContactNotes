namespace ContactNotesAPI.DTOs
{
    public class CreateNoteDto
    {
        public Guid ContactId { get; set; }
        public string Body { get; set; }
    }
}
