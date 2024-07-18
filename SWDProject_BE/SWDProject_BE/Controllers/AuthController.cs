using BusinessLayer.RequestModels;
using BusinessLayer.Services;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SWDProject_BE.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthServices _authService;

		public AuthController(IAuthServices authServices)
		{
			_authService = authServices;
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login(LoginModel model)
		{
			var result = await _authService.AuthenticateAsync(model.Email, model.Password);

			return StatusCode((int)result.Code, result);
		}

		[HttpPost("send-email")]
		public async Task<ActionResult> Gets(int userId)
		{
			var result = await _authService.SendAccount(userId);
			return StatusCode((int)result.Code, result);
		}

		[HttpPut("forgot-password")]
		public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
		{
			var result = await _authService.ForgotPassword(request);
			return StatusCode((int)result.Code, result);
		}

		[HttpPost("register")]
		public IActionResult Register(RegisterModel model)
		{
			// Implement user registration logic here

			// Once the user is registered, generate JWT token
			//return Ok(_authService.RegisterAsync(model).Result);
			var result = _authService.RegisterAsync(model).Result;
			return StatusCode((int) result.Code, result);
		}

		[HttpPost("admin-create-account")]
		[Authorize(Roles ="admin")]
		public IActionResult AdminGenAcc(AdminCreateAccountModel model)
		{
			// Implement user registration logic here

			// Once the user is registered, generate JWT token
			//return Ok(_authService.RegisterAsync(model).Result);
			var result = _authService.AdminGenAcc(model).Result;
			return StatusCode((int) result.Code, result);
		}
	}
}
