using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace Blog.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly IUserRepository userRepository;
		private readonly UserManager<IdentityUser> userManager;

		public UsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
		{
			this.userRepository = userRepository;
			this.userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> List()
		{
			var users = await userRepository.GetAll();

			var usersViewModel = new UserViewModel();
			usersViewModel.Users = new List<User>();
			foreach (var user in users)
			{
				usersViewModel.Users.Add(new User
				{
					Id = Guid.Parse(user.Id),
					UserName = user.UserName,
					Email = user.Email
				});
			}

			return View(usersViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> List(UserViewModel request)
		{
			var identityUser = new IdentityUser
			{
				UserName = request.UserName,
				Email = request.Email
			};

			var identityResult = await userManager.CreateAsync(identityUser, request.Password);

			if(identityResult is not null)
			{
				if(identityResult.Succeeded)
				{
					var roles = new List<string> { "User" };

					if(request.IsAdmin)
					{
						roles.Add("Admin");
					}

					identityResult = await userManager.AddToRolesAsync(identityUser, roles);

					if(identityResult is not null && identityResult.Succeeded)
					{
						return RedirectToAction("List", "Users");
					}
				}
			}

			return View(null);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			var user = await userManager.FindByIdAsync(id.ToString());

			if(user is not null)
			{
				var identityResult = await userManager.DeleteAsync(user);

				if(identityResult is not null && identityResult.Succeeded)
				{
					return RedirectToAction("List", "Users");
				}
			}

			return View();
		}
	}
}
