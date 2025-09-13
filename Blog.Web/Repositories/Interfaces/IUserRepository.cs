using Microsoft.AspNetCore.Identity;

namespace Blog.Web.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<IEnumerable<IdentityUser>> GetAll();
	}
}
