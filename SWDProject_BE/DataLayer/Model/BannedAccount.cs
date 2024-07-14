using System;
using System.Collections.Generic;

namespace DataLayer.Model;

public partial class BannedAccount
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime Date { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Appeal> Appeals { get; set; } = new List<Appeal>();

    public virtual User User { get; set; } = null!;
}
