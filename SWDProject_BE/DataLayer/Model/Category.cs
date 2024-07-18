using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
            SubCategories = new HashSet<SubCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}
