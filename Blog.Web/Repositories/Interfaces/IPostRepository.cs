using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories.Interfaces
{
	public interface IPostRepository
	{
		Task<IEnumerable<Post>> GetAllAsync();

		Task<Post?> GetAsync(Guid id);

		Task<Post?> GetByUrlHandleAsync(string urlHandle);

		Task<Post> AddAsync(Post post);

		Task<Post?> UpdateAsync(Post post);

		Task<Post?> DeleteAsync(Guid id);
	}
}
