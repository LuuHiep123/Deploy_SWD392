using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.Product
{
    public class UpdateProductDto
    {
        public int? CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public string? Name { get; set; } = null!;
        public double? Price { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? UrlImg { get; set; }
        public bool? Status { get; set; }

        public UpdateProductDto(int? categoryId, int? subcategoryId, string? name, double? price, string? description, string? location, string? urlImg, bool? status)
        {
            CategoryId = categoryId;
            SubcategoryId = subcategoryId;
            Name = name;
            Price = price;
            Description = description;
            Location = location;
            UrlImg = urlImg;
            Status = status;
        }
    }
}
