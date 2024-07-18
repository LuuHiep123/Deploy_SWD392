using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Date { get; set; } = null!;
        public double Amount { get; set; }
        public string Method { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
