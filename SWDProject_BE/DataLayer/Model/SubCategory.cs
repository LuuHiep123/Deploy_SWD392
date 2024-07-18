using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class SubCategory
    {
        public SubCategory()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
