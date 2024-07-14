using System;
using System.Collections.Generic;

namespace DataLayer.Model;

public partial class Group
{
    public int Id { get; set; }

    public int PostId { get; set; }

    public int UserExchangeId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual Post Post { get; set; } = null!;

    public virtual User UserExchange { get; set; } = null!;
}
