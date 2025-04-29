using System;
using System.Collections.Generic;

namespace ContactNotesAPI.Models;

public partial class Contact
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
