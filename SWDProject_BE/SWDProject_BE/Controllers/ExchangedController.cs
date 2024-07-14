using BusinessLayer.RequestModels;
using BusinessLayer.Services;
using BusinessLayer.Services.Implements;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWDProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangedController : ControllerBase
    {
        private readonly IExchangedService _exchangedService;
        private readonly IPostService _postService;

        public ExchangedController(IExchangedService exchangedService, IPostService postService)
        {
            _exchangedService = exchangedService;
            _postService = postService;
        }

        [HttpGet("GetAllFinishedForUser")]
        public async Task<ActionResult<Exchanged>> GetAllFinishedExchangedByUserId()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }

                var userId = int.Parse(userIdClaim.Value);
                var exchanged = await _exchangedService.GetAllFinishedExchangedByUserIdAsync(userId);
                return Ok(exchanged);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Exchanged>> GetExchangedById(int id)
        {
            try
            {
                var exchanged = await _exchangedService.GetExchangedByIdAsync(id);
                if (exchanged == null)
                {
                    return NotFound(new { message = "Exchanged ID not found." });
                }

                return Ok(exchanged);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllPendingForCustomer")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Exchanged>>> GetAllPendingExchangedByUserIdForCustomer()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }

                var userId = int.Parse(userIdClaim.Value);
                var exchanged = await _exchangedService.GetAllPendingExchangedByUserIdForCustomerAsync(userId);
                return Ok(exchanged);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllPendingForPoster")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Exchanged>>> GetAllPendingExchangedByUserIdForPoster()
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }

                var userId = int.Parse(userIdClaim.Value);
                var exchanged = await _exchangedService.GetAllPendingExchangedByUserIdForPosterAsync(userId);
                return Ok(exchanged);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddExchanged(ExchangedRequestModel exchangedRequest)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(exchangedRequest.PostId);
                if (post.PublicStatus == false)
                {
                    return BadRequest(new { message = "Cannot add exchange because the post is unpublish or reported." });
                }
                if (post.ExchangedStatus == true)
                {
                    return BadRequest(new { message = "Cannot add exchange because the post is already exchanged." });
                }

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }
                var userId = int.Parse(userIdClaim.Value);

                if (post.UserId == userId)
                {
                    return BadRequest(new { message = "This post is your own post." });
                }

                var exchanged = new Exchanged
                {
                    UserId = userId,
                    PostId = exchangedRequest.PostId,
                    Description = exchangedRequest.Description,
                    Date = DateTime.Now,
                    Status = false
                };
                await _exchangedService.AddExchangedAsync(exchanged);

                try
                {
                    foreach (var productId in exchangedRequest.ProductIds)
                    {
                        var exchangedProduct = new ExchangedProduct
                        {
                            ExchangeId = exchanged.Id,
                            ProductId = productId
                        };
                        await _exchangedService.AddExchangedProductAsync(exchangedProduct);
                    }
                }
                catch (Exception ex)
                {
                    // If adding exchanged product fails, delete the newly added exchanged
                    await _exchangedService.DeleteExchangedAsync(exchanged.Id);
                    return BadRequest(new { message = $"An error occurred: {ex.Message}" });
                }

                return Ok(new { message = "Exchange created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpPut("accept/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateExchangedStatusAccept(int id)
        {
            try
            {
                var existingExchanged = await _exchangedService.GetExchangedByIdAsync(id);
                var ownPost = await _postService.GetPostByIdAsync(existingExchanged.PostId);

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }
                var userId = int.Parse(userIdClaim.Value);

                if (ownPost.UserId != userId)
                {
                    return BadRequest(new { message = "This post is not your own post." });
                }

                await _exchangedService.UpdateExchangedStatusAcceptAsync(id);
                return Ok(new { message = "Exchange accepted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deny/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateExchangedStatusDeny(int id)
        {
            try
            {
                var existingExchanged = await _exchangedService.GetExchangedByIdAsync(id);
                var ownPost = await _postService.GetPostByIdAsync(existingExchanged.PostId);

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }
                var userId = int.Parse(userIdClaim.Value);

                if (ownPost.UserId != userId)
                {
                    return BadRequest(new { message = "This post is not your own post." });
                }

                await _exchangedService.UpdateExchangedStatusDenyAsync(id);
                return Ok(new { message = "Exchange denied successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("cancel/{id}")]
        [Authorize]
        public async Task<ActionResult> CancelExchanged(int id)
        {
            try
            {
                var existingExchanged = await _exchangedService.GetExchangedByIdAsync(id);

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }

                var userId = int.Parse(userIdClaim.Value);
                if (existingExchanged.UserId != userId)
                {
                    return BadRequest(new { message = "This exchange is not yours." });
                }

                await _exchangedService.UpdateExchangedStatusDenyAsync(id);
                return Ok(new { message = "Exchange cancelled successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
