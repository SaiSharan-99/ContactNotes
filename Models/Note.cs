using System;
using System.Collections.Generic;

namespace ContactNotesAPI.Models;

public partial class Note
{
    public Guid Id { get; set; }

    public Guid ContactId { get; set; }

    public string Body { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Contact Contact { get; set; } = null!;
}
