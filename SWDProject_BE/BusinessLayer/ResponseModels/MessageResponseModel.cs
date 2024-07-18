using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels
{
	public class MessageResponseModel
	{
		public int Id { get; set; }

		public int SenderId { get; set; }

		public int GroupId { get; set; }

		public string Content { get; set; } = null!;

		public DateTime CreatedDate { get; set; }

		public DateTime? ModifiedDate { get; set; }
	}
}
