﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[DisplayName("Username")]
		public string UserName { get; set; }
		[Required]
        [MinLength(6, ErrorMessage = "Password has to be at least 6 characters.")]
        public string Password { get; set; }
		public string? ReturnUrl { get; set; }
	}
}
