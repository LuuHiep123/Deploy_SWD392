using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class ExchangedProduct
    {
        public int Id { get; set; }
        public int ExchangeId { get; set; }
        public int ProductId { get; set; }

        public virtual Exchanged Exchange { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
