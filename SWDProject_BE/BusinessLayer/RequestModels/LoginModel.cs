﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModels
{
	public class LoginModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class ForgotPasswordRequest 
	{
		[Required]
		public string Email { get; set; } = null!;
	}

}
