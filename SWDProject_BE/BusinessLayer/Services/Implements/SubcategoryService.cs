using AutoMapper;
using BusinessLayer.RequestModels.Subcategory;
using BusinessLayer.ResponseModels.Category;
using BusinessLayer.ResponseModels.Subcategory;
using DataLayer.Model;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implements
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubcategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<string> AddSubCategory(SubCategoryRequestModel dto)
        {
            try
            {
                SubCategory subCategory = new SubCategory()
                {
                    CategoryId = dto.CategoryId,
                    Name = dto.Name,
                    Description = dto.Description,
                    Status = true
                };
                await _unitOfWork.Repository<SubCategory>().InsertAsync(subCategory);
                await _unitOfWork.CommitAsync();
                return "Add SubCategory Sucessfull";

            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<string> DeleteSubCategory(int id)
        {
            try
            {
                var subCategory = await _unitOfWork.Repository<SubCategory>().GetById(id);
                if (subCategory != null)
                {
                    subCategory.Status = false;
                    await _unitOfWork.Repository<SubCategory>().Update(subCategory, id);
                    await _unitOfWork.CommitAsync();
                    return "Delete Successful!";
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<List<SubcategoryResponseModel>> GetAll()
        {
            try
            {
                List<SubCategory> listSubcategory = new List<SubCategory>();
                var subCategory = await _unitOfWork.Repository<SubCategory>().GetAll().ToListAsync();
                var category = await _unitOfWork.Repository<Category>().GetAll().ToListAsync();
                foreach (var sub in subCategory)
                {
                    foreach (var ca in category)
                    {
                        if(sub.Id == ca.Id)
                        {
                            listSubcategory.Add(sub);
                        }
                    }
                }
                var result = _mapper.Map<List<SubcategoryResponseModel>>(listSubcategory);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<List<SubcategoryResponseModel>> GetAllValidSubCategory()
        {
            try
            {
                List<SubCategory> listSubcategory = new List<SubCategory>();
                var subCategory = await _unitOfWork.Repository<SubCategory>().GetAll().Where(s => s.Status == true).ToListAsync();
                var category = await _unitOfWork.Repository<Category>().GetAll().ToListAsync();
                foreach (var sub in subCategory)
                {
                    foreach (var ca in category)
                    {
                        if (sub.Id == ca.Id)
                        {
                            listSubcategory.Add(sub);
                        }
                    }
                }
                var result = _mapper.Map<List<SubcategoryResponseModel>>(listSubcategory);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<SubcategoryResponseModel> GetById(int id)
        {
            try
            {
               
                var subCategory = await _unitOfWork.Repository<SubCategory>().GetById(id);
                if (subCategory != null)
                {
                    var category = await _unitOfWork.Repository<Category>().GetAll().ToListAsync();
                    foreach (var ca in category)
                    {
                        if (subCategory.CategoryId == ca.Id)
                        {
                            var result = _mapper.Map<SubcategoryResponseModel>(subCategory);
                            return result;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error DB!");
            }
        }

        public async Task<List<SubcategoryResponseModel>> GetSubcategoryByCategoryId(int categoryId)
        {
            var category = await _unitOfWork.Repository<Category>().GetById(categoryId);
            if(category != null)
            {
                var subcategory = await _unitOfWork.Repository<SubCategory>().GetAll().Where(s => s.CategoryId == categoryId && s.Status == true).ToListAsync();
                var result = _mapper.Map<List<SubcategoryResponseModel>>(subcategory);
                return result;
            }                   
            return null;
        }

        public async Task<string> UpdateSubCategory(int id, SubCategoryRequestModel dto)
        {
            try
            {
                var subCategory = await _unitOfWork.Repository<SubCategory>().GetById(id);
                if (subCategory != null)
                {
                    if(dto.CategoryId != 0)
                    {
                        subCategory.CategoryId = dto.CategoryId;
                    }
                    if (dto.Name != null)
                    {
                        subCategory.Name = dto.Name;
                    }
                    if (dto.Description != null)
                    {
                        subCategory.Description = dto.Description;
                    }
                    await _unitOfWork.Repository<SubCategory>().Update(subCategory, id);
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
