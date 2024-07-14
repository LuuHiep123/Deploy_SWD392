using BusinessLayer.RequestModels.Category;
using BusinessLayer.RequestModels.Subcategory;
using BusinessLayer.Services;
using BusinessLayer.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWDProject_BE.Controllers
{
    [Route("api/Subcategory")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService _subCategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subCategoryService = subcategoryService;
        }

        [HttpGet]
        [Route("GetAllSubCategory")]
        public async Task<IActionResult> GetAllSubCategory()
        {
            try
            {
                var subCategory = await _subCategoryService.GetAll();
                return Ok(subCategory);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAllValidSubCategory")]
        public async Task<IActionResult> GetAllValidSubCategory()
        {
            try
            {
                var subCategory = await _subCategoryService.GetAllValidSubCategory();
                return Ok(subCategory);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        [Route("GetSubcategoryById/{id}")]
        public async Task<IActionResult> GetSubcategoryById(int id)
        {
            try
            {
                var subCategory = await _subCategoryService.GetById(id);
                if(subCategory != null)
                {
                    return Ok(subCategory);
                }
                else
                {
                    return NotFound();
                }
                

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetSubcategoryByCategoryId/{categoryId}")]
        public async Task<IActionResult> GetSubcategoryByCategoryId(int categoryId)
        {
            try
            {
                var subCategory = await _subCategoryService.GetSubcategoryByCategoryId(categoryId);
                if (subCategory != null)
                {
                    return Ok(subCategory);
                }
                else
                {
                    return NotFound("not found Category!");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddSubCategory")]
        public async Task<IActionResult> AddSubCategory(SubCategoryRequestModel dto)
        {
            try
            {
                String message = await _subCategoryService.AddSubCategory(dto);
                return Ok(message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateSubCategory/{id}")]
        public async Task<IActionResult> UpdateSubCategory(int id, SubCategoryRequestModel dto)
        {
            try
            {
                String message = await _subCategoryService.UpdateSubCategory(id, dto);
                if (message != null)
                {
                    return Ok(message);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("DeleteSubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            try
            {
                String message = await _subCategoryService.DeleteSubCategory(id);
                if (message != null)
                {
                    return Ok(message);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
