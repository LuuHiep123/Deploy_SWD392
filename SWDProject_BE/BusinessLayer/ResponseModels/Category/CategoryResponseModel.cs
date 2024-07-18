using BusinessLayer.ResponseModels.Subcategory;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.Category
{
    public class CategoryResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<SubcategoryResponseModel> SubCategories { get; set; }
    }

    public class CategoryResponseModelForStaff : CategoryResponseModel
    {
        public bool Status { get; set; }
    }
}
