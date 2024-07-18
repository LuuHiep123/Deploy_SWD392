using BusinessLayer.RequestModels.Order;
using BusinessLayer.ResponseModels.Order;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IOrderService
    {
        Task<List<OrderResponseModel>> GetAllOrder();
    }
}
