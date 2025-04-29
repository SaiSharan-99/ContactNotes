namespace ContactNotesAPI.DTOs
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}
