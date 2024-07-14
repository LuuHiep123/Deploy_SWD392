using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels.Appeal
{
    public class AddAppealRequestModel
    {
        public int? BannerAcountId { get; set; }
        public string Description { get; set; } = null!;
    }
}
