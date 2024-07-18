using BusinessLayer.RequestModels.Appeal;
using BusinessLayer.Services;
using DataLayer.Dto.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWDProject_BE.Controllers
{
    [Route("api/Appeal/")]
    [ApiController]
    public class AppealController : ControllerBase
    {
        private readonly IAppealService _appealService;

        public AppealController(IAppealService appealService)
        {
            _appealService = appealService;
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _appealService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        [Route("GetAppealById/{AppealId}")]
        [Authorize]
        public async Task<IActionResult> GetAppealById(int AppealId)
        {
            try
            {
                var result = await _appealService.FindAppealById(AppealId);
                if(result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound("Not fount Appeal!");
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllAppealProcessing")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> GetAllAppealProcessing()
        {
            try
            {
                var result = await _appealService.GetAllAppealProcessingll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetAllByUserId/{UserId}")]
        public async Task<IActionResult> GetAllByUserId(int UserId)
        {
            try
            {
                var result = await _appealService.GetAllByUserId(UserId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("not found User!");
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllByBannerAccountId/{BannerAccountId}")]
        public async Task<IActionResult> GetAllByBannerAccountId(int BannerAccountId)
        {
            try
            {
                var result = await _appealService.GetAllByBannerAccountId(BannerAccountId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("not found BannerAccount!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AcceptAppeal/{AppealId}")]
        public async Task<IActionResult> AcceptAppeal(int AppealId)
        {
            try
            {
                var result = await _appealService.AcceptAppeal(AppealId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("not found Appeal!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddAppeal")]
        public async Task<IActionResult> AddAppeal(AddAppealRequestModel dto, int userId)
        {
            try
            {

                String message = await _appealService.AddAppeal(dto, userId);
                return Ok(message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateAppeal/{AppealId}")]
        public async Task<IActionResult> UpdateAppeal(UppdateAppealRequestModel dto, int AppealId)
        {
            try
            {
                var message = await _appealService.UpdateAppeal(dto , AppealId);
                if (message != null)
                {
                    return Ok(message);
                }
                return NotFound("not found Appeal!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
