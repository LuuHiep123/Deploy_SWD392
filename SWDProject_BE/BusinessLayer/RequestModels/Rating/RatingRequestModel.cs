using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.Rating
{
    public class RatingRequestModel
    {
        public int PostId { get; set; }
        public int Score { get; set; }
        public string? Description { get; set; }
    }
}
