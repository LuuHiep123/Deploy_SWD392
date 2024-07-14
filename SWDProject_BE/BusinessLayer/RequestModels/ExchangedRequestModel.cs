using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels
{
    public class ExchangedRequestModel
    {

        public int PostId { get; set; }

        public string? Description { get; set; }

        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
