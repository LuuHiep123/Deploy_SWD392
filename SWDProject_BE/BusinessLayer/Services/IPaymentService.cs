using BusinessLayer.RequestModels;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Payments;
using System.Threading.Tasks;

public interface IPaymentService
{
    Task<Order> CreatePaymentAsync(decimal amount, string returnUrl, string cancelUrl);
    Task ExecutePaymentAsync(string paymentId, string payerId, OrderRequestModel orderRequest, int userId);
    Task<PayPalCheckoutSdk.Payments.Refund> RefundPaymentAsync(string captureId, decimal amount);
}
