using BusinessLayer.RequestModels.Category;
using BusinessLayer.RequestModels.Subcategory;
using BusinessLayer.ResponseModels.Category;
using BusinessLayer.ResponseModels.Subcategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface ISubcategoryService
    {
        Task<string> UpdateSubCategory(int id, SubCategoryRequestModel dto);
        Task<string> AddSubCategory(SubCategoryRequestModel dto);
        Task<string> DeleteSubCategory(int id);
        Task<List<SubcategoryResponseModel>> GetAll();
        Task<List<SubcategoryResponseModel>> GetAllValidSubCategory();
        Task<SubcategoryResponseModel> GetById(int id);
        Task<List<SubcategoryResponseModel>> GetSubcategoryByCategoryId(int categoryId);
    }
}
