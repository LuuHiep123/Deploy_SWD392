using BusinessLayer.RequestModels;
using DataLayer.Model;
using DataLayer.Repository;
using DataLayer.UnitOfWork;

public class PaymentService : IPaymentService
{
    private readonly PayPalService _payPalService;
    private readonly IUnitOfWork _unitOfWork;


    public PaymentService(IUnitOfWork unitOfWork, PayPalService payPalService)
    {
        _payPalService = payPalService;
        _unitOfWork = unitOfWork;
    }

    public async Task<PayPalCheckoutSdk.Orders.Order> CreatePaymentAsync(decimal amount, string returnUrl, string cancelUrl)
    {
        var payment = await _payPalService.CreateOrder(amount, returnUrl, cancelUrl);
        return payment;
    }

    public async Task ExecutePaymentAsync(string paymentId, string payerId, OrderRequestModel orderRequest, int userId)
    {
        var dbContextField = _unitOfWork.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var dbContext = (SWD392_DBContext)dbContextField.GetValue(_unitOfWork);

        using (var transaction = await dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var paymentStatus = await _payPalService.GetPaymentStatus(paymentId);
                if (paymentStatus == "COMPLETED")
                {
                    var order = new DataLayer.Model.Order
                    {
                        UserId = userId,
                        PaymentId = null, // add later
                        TotalPrice = orderRequest.TotalPrice,
                        Date = DateTime.Now,
                        Status = false
                    };
                    await _unitOfWork.Repository<DataLayer.Model.Order>().InsertAsync(order);
                    await _unitOfWork.CommitAsync();

                    var payment = new Payment
                    {
                        Date = DateTime.Now.ToString("yyyy-MM-dd"),
                        Amount = orderRequest.TotalPrice,
                        Method = "PayPal",
                        Status = true,
                        Description = "PayerId: " + payerId + " - PaymentId: " + paymentId,
                    };
                    await _unitOfWork.Repository<Payment>().InsertAsync(payment);
                    await _unitOfWork.CommitAsync();

                    order.PaymentId = payment.Id;
                    await _unitOfWork.Repository<DataLayer.Model.Order>().Update(order, order.Id);
                    await _unitOfWork.CommitAsync();

                    foreach (var detail in orderRequest.OrderDetails)
                    {
                        var product = await _unitOfWork.Repository<Product>().FindAsync(p => p.Id == detail.ProductId && p.Status == true);
                        if (product == null)
                        {
                            throw new Exception("Product is either not exist or is sold.");
                        }
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ProductId = detail.ProductId,
                            Price = detail.Price,
                            Status = true
                        };

                        await _unitOfWork.Repository<DataLayer.Model.OrderDetail>().InsertAsync(orderDetail);
                        product.Status = false;
                        await _unitOfWork.Repository<Product>().Update(product, product.Id);
                        await _unitOfWork.CommitAsync();
                    }

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<PayPalCheckoutSdk.Payments.Refund> RefundPaymentAsync(string captureId, decimal amount)
    {
        var refund = await _payPalService.RefundPayment(captureId, amount);

        var payment = _unitOfWork.Repository<Payment>().GetAll().FirstOrDefault(p => p.Description == captureId);
        if (payment == null) throw new Exception("Payment not found");

        payment.Status = false;
        await _unitOfWork.Repository<Payment>().Update(payment, payment.Id);
        await _unitOfWork.CommitAsync();

        return refund;
    }
}
