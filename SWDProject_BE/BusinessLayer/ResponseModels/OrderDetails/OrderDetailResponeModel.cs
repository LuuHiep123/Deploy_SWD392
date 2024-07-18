using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels
{
    public class OrderDetailResponeModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImgUrl { get; set; }

        public double Price { get; set; }

        public bool Status { get; set; }
    }
}
