using AutoMapper;
using BusinessLayer.RequestModels.Category;
using BusinessLayer.ResponseModels.Category;
using BusinessLayer.ResponseModels.Subcategory;
using DataLayer.Model;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork; 
            _mapper = mapper;
        }

        public async Task<string> AddCategory(CategoryRequestModel dto)
        {
            try
            {
                Category category = new Category()
                {
                    Description = dto.Description,
                    Name = dto.Name,
                    Status = true
                };
                var exsitingCategory = await _unitOfWork.Repository<Category>().FindAsync(c => c.Name.Equals(category.Name));
                if (exsitingCategory != null)
                {
                    return ("Category "+ category.Name + " exsited");
                }
                await _unitOfWork.Repository<Category>().InsertAsync(category);
                await _unitOfWork.CommitAsync();
                return "Add Category Sucessfull";

            } catch (Exception ex) 
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<string> DeleteCategory(int id)
        {
            try
            {
                var category = await _unitOfWork.Repository<Category>().GetById(id);
                if (category != null)
                {
                    category.Status = false;
                    await _unitOfWork.Repository<Category>().Update(category,id);
                    await _unitOfWork.CommitAsync();

                    var subCategories = await _unitOfWork.Repository<SubCategory>().GetAll().Where(sc => sc.CategoryId == id).ToListAsync();
                    foreach (var subCategory in subCategories)
                    {
                        subCategory.Status = false;
                        await _unitOfWork.Repository<SubCategory>().Update(subCategory, subCategory.Id);
                        await _unitOfWork.CommitAsync();
                    }


                    return "Delete Successful!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }

        }

        public async Task<List<CategoryResponse>> GetAll()
        {
            try
            {
                var category = await _unitOfWork.Repository<Category>().GetAll().ToListAsync();
                var result = _mapper.Map<List<CategoryResponse>>(category);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<List<CategoryResponse>> GetAllValidCategory()
        {
            try
            {
                List<CategoryResponse> result = new List<CategoryResponse>();

                var listCategory = await _unitOfWork.Repository<Category>()
                    .GetAll()
                    .Include(c => c.SubCategories) 
                    .Where(c => c.Status == true)
                    .ToListAsync();

                foreach (var category in listCategory)
                {
                    var validSubCategories = category.SubCategories.Where(s => s.Status == true).ToList();
                    if (validSubCategories.Any())
                    {
                        CategoryResponse categoryResponseModel = new CategoryResponse
                        {
                            Id = category.Id,
                            Name = category.Name,
                            Description = category.Description,
                            Status = category.Status
                        };
                        result.Add(categoryResponseModel);
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<List<CategoryResponseModel>> GetAllWithSubcategoryForUser()
        {
            try
            {
                List<CategoryResponseModel> result = new List<CategoryResponseModel>();
                var listCategory = await _unitOfWork.Repository<Category>()
                    .GetAll()
                    .Include(c => c.SubCategories)
                    .Where(c => c.Status == true)
                    .ToListAsync();

                foreach (var category in listCategory)
                {
                    var validSubCategories = category.SubCategories.Where(s => s.Status == true).ToList();
                    if (validSubCategories.Any())
                    {
                        var listSubcategoryResponse = _mapper.Map<List<SubcategoryResponseModel>>(validSubCategories);

                        CategoryResponseModel categoryResponseModel = new CategoryResponseModel
                        {
                            Id = category.Id,
                            Name = category.Name,
                            SubCategories = listSubcategoryResponse
                        };
                        result.Add(categoryResponseModel);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CategoryResponseModelForStaff>> GetAllWithSubcategoryForStaff()
        {
            try
            {
                List<CategoryResponseModelForStaff> result = new List<CategoryResponseModelForStaff>();
                var listCategory = await _unitOfWork.Repository<Category>()
                    .GetAll()
                    .Include(c => c.SubCategories)
                    .ToListAsync();

                foreach (var category in listCategory)
                {
                    var listSubcategoryResponse = _mapper.Map<List<SubcategoryResponseModel>>(category.SubCategories);

                    CategoryResponseModelForStaff categoryResponseModel = new CategoryResponseModelForStaff
                    {
                        Id = category.Id,
                        Name = category.Name,
                        SubCategories = listSubcategoryResponse,
                        Status = category.Status
                    };

                    result.Add(categoryResponseModel);

                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<CategoryResponse> GetById(int id)
        {
            try
            {
                var category = await _unitOfWork.Repository<Category>().GetById(id);
                if(category != null)
                {
                    var result = _mapper.Map<CategoryResponse>(category);
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UpdateCategory(int id, CategoryRequestModel dto)
        {
            try
            {
                var category = await _unitOfWork.Repository<Category>().GetById(id);
                
                if (category != null)
                {
                    var exsitingCategory = await _unitOfWork.Repository<Category>().FindAsync(c => c.Name.Equals(dto.Name));
                    if (exsitingCategory != null)
                    {
                        return ("Category " + category.Name + " exsited");
                    }

                    if (dto.Name != null)
                    {
                        category.Name = dto.Name;
                    }
                    if (dto.Description != null)
                    {
                        category.Description = dto.Description;
                    }
                    await _unitOfWork.Repository<Category>().Update(category, id);
                    await _unitOfWork.CommitAsync();
                    return "Update Successful!";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }

        }
    }
}
