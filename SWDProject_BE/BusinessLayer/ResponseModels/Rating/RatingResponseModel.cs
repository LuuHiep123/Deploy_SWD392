using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.Rating
{
    public class RatingResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserUrl { get; set; }
        public int PostId { get; set; }
        public int Score { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
