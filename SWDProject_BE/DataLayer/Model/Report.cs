using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Report
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
