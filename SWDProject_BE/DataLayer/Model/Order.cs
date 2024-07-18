using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int? PaymentId { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public virtual Payment? Payment { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
