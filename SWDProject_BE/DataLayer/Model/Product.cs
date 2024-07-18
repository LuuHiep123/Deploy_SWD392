using System;
using System.Collections.Generic;

namespace DataLayer.Model
{
    public partial class Product
    {
        public Product()
        {
            ExchangedProducts = new HashSet<ExchangedProduct>();
            OrderDetails = new HashSet<OrderDetail>();
            Posts = new HashSet<Post>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? UrlImg { get; set; }
        public bool Status { get; set; }
        public bool IsForSell { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual SubCategory SubCategory { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ExchangedProduct> ExchangedProducts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
