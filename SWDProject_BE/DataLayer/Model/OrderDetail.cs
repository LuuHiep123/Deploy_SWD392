using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public bool Status { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
