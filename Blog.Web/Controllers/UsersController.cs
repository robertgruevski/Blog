using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly IUserRepository userRepository;

		public UsersController(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
		}
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
	}
}
