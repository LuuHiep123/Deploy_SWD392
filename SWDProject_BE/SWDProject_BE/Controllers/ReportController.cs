using BusinessLayer.RequestModels.Product;
using BusinessLayer.RequestModels.Report;
using BusinessLayer.Services;
using DataLayer.Dto.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWDProject_BE.Controllers
{
    [Route("api/Report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("GetAllReport")]
        public async Task<IActionResult> GetAllReport()
        {
            try
            {
                var report = await _reportService.GetAll();
                return Ok(report);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAllValidReport")]
        public async Task<IActionResult> GetAllValidReport()
        {
            try
            {
                var report = await _reportService.GetAllValidReport();
                return Ok(report);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetAllReportByUserId/{UserId}")]
        public async Task<IActionResult> GetAllReportByUserId(int UserId)
        {
            try
            {
                var report = await _reportService.GetReportByUserId(UserId);
                if(report != null)
                {
                    return Ok(report);
                }
                else
                {
                    return NotFound("Not found UserId!");
                }
                

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet]
        [Route("GetAllReportByPostrId/{PostId}")]
        public async Task<IActionResult> GetAllReportByPostrId(int PostId)
        {
            try
            {
                var report = await _reportService.GetReportByPostId(PostId);
                if (report != null)
                {
                    return Ok(report);
                }
                else
                {
                    return NotFound("Not found PostId!");
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("AddReport")]
        public async Task<IActionResult> AddReport(ReportRequestaUser dto)
        {
            try
            {              
                // Take the user id from JWT
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                var userId = int.Parse(userIdClaim.Value);

                string message = await _reportService.AddReportByUser(dto, userId);
                if (message != null)
                {
                    return Ok(message);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateReport/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ReportRequestaUser dto)
        {
            try
            {
                String message = await _reportService.UpdateReportByUser(id, dto);
                if (message != null)
                {
                    return Ok(message);
                }
                else
                {
                    return NotFound("Not found Report!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteReport/{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            try
            {
                String message = await _reportService.DeleteReport(id);
                if (message != null)
                {
                    return Ok(message);
                }
                else
                {
                    return NotFound("Not found Report!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("AcceptReport/{id}")]
        public async Task<IActionResult> AcceptReport(int id)
        {
            try
            {
                String message = await _reportService.AcceptReport(id);
                if (message != null)
                {
                    return Ok(message);
                }
                else
                {
                    return NotFound("Not found Report!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
