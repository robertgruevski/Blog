using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories.Interfaces
{
	public interface ICommentRepository
	{
		Task<Comment> AddAsync(Comment comment);
	}
}
