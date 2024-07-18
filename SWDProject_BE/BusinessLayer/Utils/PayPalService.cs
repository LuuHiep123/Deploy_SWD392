using BusinessLayer.Utils;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Payments;
using System.Threading.Tasks;

public class PayPalService
{
    private readonly PayPalHttpClient _client;

    public PayPalService()
    {
        _client = PayPalConfig.GetClient();
    }

    public async Task<Order> CreateOrder(decimal amount, string returnUrl, string cancelUrl)
    {
        var order = new OrderRequest
        {
            CheckoutPaymentIntent = "CAPTURE",
            ApplicationContext = new ApplicationContext
            {
                ReturnUrl = returnUrl,
                CancelUrl = cancelUrl
            },
            PurchaseUnits = new List<PurchaseUnitRequest>
            {
                new PurchaseUnitRequest
                {
                    AmountWithBreakdown = new AmountWithBreakdown
                    {
                        CurrencyCode = "USD",
                        Value = amount.ToString("F")
                    }
                }
            }
        };

        var request = new OrdersCreateRequest();
        request.RequestBody(order);

        var response = await _client.Execute(request);
        var result = response.Result<Order>();
        return result;
    }

    public async Task<Order> CaptureOrder(string orderId)
    {
        var request = new OrdersCaptureRequest(orderId);
        request.RequestBody(new OrderActionRequest());
        var response = await _client.Execute(request);
        var result = response.Result<Order>();
        return result;
    }

    public async Task<PayPalCheckoutSdk.Payments.Refund> RefundPayment(string captureId, decimal amount)
    {
        var request = new CapturesRefundRequest(captureId);
        request.RequestBody(new RefundRequest
        {
            Amount = new PayPalCheckoutSdk.Payments.Money
            {
                CurrencyCode = "USD",
                Value = amount.ToString("F")
            }
        });

        var response = await _client.Execute(request);
        var result = response.Result<PayPalCheckoutSdk.Payments.Refund>();
        return result;
    }

    public async Task<string> GetPaymentStatus(string orderId)
    {
        var request = new OrdersGetRequest(orderId);
        var response = await _client.Execute(request);
        var result = response.Result<Order>();

        return result.Status;
    }
}
