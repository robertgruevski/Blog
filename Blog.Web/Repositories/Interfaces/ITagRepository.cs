using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories.Interfaces
{
	public interface ITagRepository
	{
		Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery = null);
		Task<Tag?> GetAsync(Guid id);
		Task<Tag> AddAsync(Tag id);
		Task<Tag?> UpdateAsync(Tag tag);
		Task<Tag?> DeleteAsync(Guid id);
    }
}
