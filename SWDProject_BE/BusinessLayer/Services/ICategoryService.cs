using BusinessLayer.RequestModels.Category;
using BusinessLayer.ResponseModels.Category;
using BusinessLayer.Services.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseModel>> GetAllWithSubcategoryForUser();
        Task<List<CategoryResponseModelForStaff>> GetAllWithSubcategoryForStaff();
        Task<string> UpdateCategory(int id, CategoryRequestModel dto);
        Task<string> AddCategory(CategoryRequestModel dto);
        Task<string> DeleteCategory(int id);
        Task<List<CategoryResponse>> GetAll();
        Task<List<CategoryResponse>> GetAllValidCategory();
        Task<CategoryResponse> GetById(int id);
        
    }
}
