using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels
{
    public class ExchangedResponseModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public UserResponse User { get; set; } = null!;
        public UserResponse PostOwner { get; set; } = null!;
        public PostResponse Post { get; set; } = null!;
        public ProductResponse ProductOfPost { get; set; } = null!;
        public List<ProductResponseForExchange> ExchangedProducts { get; set; } = new List<ProductResponseForExchange>();
    }

    public class PostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
