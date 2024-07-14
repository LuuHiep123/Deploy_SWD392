using System;
using System.Collections.Generic;

namespace DataLayer.Model;

public partial class Message
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int GroupId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
