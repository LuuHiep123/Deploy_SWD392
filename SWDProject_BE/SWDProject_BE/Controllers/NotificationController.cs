using BusinessLayer.Services;
using BusinessLayer.Services.Implements;
using Microsoft.AspNetCore.Mvc;

namespace SWDProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IUsersService _userService;

        public NotificationController(INotificationService notificationService, IUsersService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }

        [HttpGet("GetNotificationByUserId/{userId}")]
        public async Task<IActionResult> GetAllCommentsByPost(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User ID not found." });
                }

                var notifications = await _notificationService.GetAllNotificationByUserId(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
