using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels
{
	public class LoginResponseModel
	{
		public string Token { get; set; }

		public UsersResponseModel User { get; set; }
    }
}
