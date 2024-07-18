using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels
{
	public class BannedUserRequestModel
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Description { get; set; } = null!;
		public DateTime Date { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public bool Status { get; set; }
	}
}
