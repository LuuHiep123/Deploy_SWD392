using BusinessLayer.RequestModels.Category;
using BusinessLayer.RequestModels.Product;
using BusinessLayer.Services;
using DataLayer.Dto.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SWDProject_BE.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("GetAllWithSubcategoryForUser")]
        public async Task<IActionResult> GetAllCategoryWithSubcategory()
        {
            try
            {
                var category = await _categoryService.GetAllWithSubcategoryForUser();
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("GetAllCategoryWithSubcategoryForStaff")]
        public async Task<IActionResult> GetAllWithSubcategoryForStaff()
        {
            try
            {
                var category = await _categoryService.GetAllWithSubcategoryForStaff();
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var category = await _categoryService.GetAll();
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAllValidCategory")]
        public async Task<IActionResult> GetAllValidCategory()
        {
            try
            {
                var category = await _categoryService.GetAllValidCategory();
                return Ok(category);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryRequestModel dto)
        {
            try
            {
                String message = await _categoryService.AddCategory(dto);
                return Ok(message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                String message = await _categoryService.DeleteCategory(id);
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

        [HttpGet]
        [Route("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetById(id);
                if (category != null)
                {
                    return Ok(category);
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
        [Route("UpdateCategory/{id}")]
        public async Task<IActionResult> UpdateCategory(int id,CategoryRequestModel dto)
        {
            try
            {
                String message = await _categoryService.UpdateCategory(id, dto);
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
