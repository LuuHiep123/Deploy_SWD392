using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Exchanged
    {
        public Exchanged()
        {
            ExchangedProducts = new HashSet<ExchangedProduct>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public bool StatusRating { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ExchangedProduct> ExchangedProducts { get; set; }
    }
}
