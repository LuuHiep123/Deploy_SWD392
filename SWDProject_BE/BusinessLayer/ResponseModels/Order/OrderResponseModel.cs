using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.Order
{
    public class OrderResponseModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }

        public int PaymentId { get; set; }
        public double Amount { get; set; }

        public double TotalPrice { get; set; }

        public DateTime Date { get; set; }

        public bool Status { get; set; }

        public List<OrderDetailResponeModel> OrderDetails { get; set; }
    }
}
