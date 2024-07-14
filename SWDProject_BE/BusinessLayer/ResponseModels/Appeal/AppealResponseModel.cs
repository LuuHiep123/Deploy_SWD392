using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.Appeal
{
    public class AppealResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BannerAcountId { get; set; }
        public string BannerDescription { get; set; }
        public DateTime BannerDate { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Status { get; set; }
    }
}
