using BusinessLayer.Services;
using DataLayer.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SWDProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannedAccountController : ControllerBase
    {
        private readonly IBannedAccountService _bannedAccountService;
        private readonly IUsersService _userService;

        public BannedAccountController(IBannedAccountService bannedAccountService, IUsersService userService)
        {
            _bannedAccountService = bannedAccountService;
            _userService = userService;
        }

        [HttpPut("BanUser/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> BanUser(int id, string reason)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User ID not found." });
                }
                if (user.Status == false)
                {
                    return BadRequest(new { message = "User Banned Already." });
                }

                await _bannedAccountService.BanUser(id, reason);

                return Ok(new { message = "User Banned successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UnBanUser/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnBanUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User ID not found." });
                }
                if (user.Status == true)
                {
                    return BadRequest(new { message = "User UnBanned Already." });
                }

                await _bannedAccountService.UnBanUser(id);

                return Ok(new { message = "User UnBanned successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
