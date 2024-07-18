using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.Subcategory
{
    public class SubcategoryResponseModel
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
