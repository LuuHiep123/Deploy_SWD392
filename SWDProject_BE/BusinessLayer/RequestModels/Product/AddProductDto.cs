using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Dto.Product
{
    public class AddProductDto
    {

        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? UrlImg { get; set; }



        public AddProductDto(int categoryId, string name, string? description, string? urlImg)
        {
            CategoryId = categoryId;
            Name = name;
            Description = description;
            UrlImg = urlImg;
        }
    }
}
