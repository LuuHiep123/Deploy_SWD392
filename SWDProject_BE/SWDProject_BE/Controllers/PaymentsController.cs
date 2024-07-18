using BusinessLayer.RequestModels;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWDProject_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePayment(decimal amount)
        {
            try
            {
                // Generate return and cancel URLs dynamically
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var returnUrl = $"{baseUrl}/api/payments/execute";
                var cancelUrl = $"{baseUrl}/api/payments/cancel";

                var payment = await _paymentService.CreatePaymentAsync(amount, returnUrl, cancelUrl);
                if (payment == null)
                {
                    return BadRequest("Unable to create payment. PayPal response is null.");
                }

                var approvalLink = payment.Links.FirstOrDefault(link => link.Rel.Equals("approve", StringComparison.OrdinalIgnoreCase))?.Href;
                if (!string.IsNullOrEmpty(approvalLink))
                {
                    return Ok(new { approvalUrl = approvalLink, paymentId = payment.Id });
                }

                return BadRequest("Unable to create payment. Approval link is missing.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating payment: {ex.Message}");
            }
        }

        [HttpPost("execute")]
        [Authorize]
        public async Task<IActionResult> ExecutePayment(string token, string PayerID, OrderRequestModel orderRequest)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "Check JWT token." });
                }
                var userId = int.Parse(userIdClaim.Value);

                await _paymentService.ExecutePaymentAsync(token, PayerID, orderRequest,userId);
                return Ok("Successfull payment");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("refund")]
        [Authorize]
        public async Task<IActionResult> RefundPayment(string captureId, decimal amount)
        {
            try
            {
                var refund = await _paymentService.RefundPaymentAsync(captureId, amount);
                return Ok(refund);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
