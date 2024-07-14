using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels
{
	public class UsersResponseModel
	{
		public int Id { get; set; }
		public string UserName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public DateTime Dob { get; set; }
		public string Address { get; set; } = null!;
		public string PhoneNumber { get; set; } = null!;
		public int RoleId { get; set; }
        public string ImgUrl { get; set; } = null!;
		public string Gender { get; set; }
	
	}
    public class UserDetailResponse : UsersResponseModel
    {
        public int? RatingCount { get; set; }
    }
}
