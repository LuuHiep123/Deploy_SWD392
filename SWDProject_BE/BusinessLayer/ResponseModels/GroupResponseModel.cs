using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels
{
    public class GroupResponseModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserExchangeId { get; set; }
    }
}
