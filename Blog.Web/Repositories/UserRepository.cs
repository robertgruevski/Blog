using Blog.Web.Data;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly AuthDbContext context;

		public UserRepository(AuthDbContext context)
		{
			this.context = context;
		}
		public async Task<IEnumerable<IdentityUser>> GetAll()
		{
			var users = await context.Users.ToListAsync();

			var superAdminUser = await context.Users
				.FirstOrDefaultAsync(x => x.Email == "superadmin@blog.com");

			if (superAdminUser is not null)
			{
				users.Remove(superAdminUser);
			}

			return users;
		}
	}
}
