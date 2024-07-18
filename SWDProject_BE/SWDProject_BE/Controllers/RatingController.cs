using BusinessLayer.RequestModels.Rating;
using BusinessLayer.Services;
using DataLayer.Dto.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWDProject_BE.Controllers
{
    [Route("api/Rating/")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        [Route("RatingExchanged")]
        public async Task<IActionResult> RatingExchanged(RatingRequestModel dto)
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

                bool check = await _ratingService.RatingPost(userId,dto);
                if (check)
                {
                    return Ok("Rating successfull!");
                }
                else
                {
                    return BadRequest("User has been rating this post!");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
