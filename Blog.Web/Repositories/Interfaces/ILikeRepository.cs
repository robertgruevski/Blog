using Blog.Web.Models.Domain;

namespace Blog.Web.Repositories.Interfaces
{
	public interface ILikeRepository
	{
		Task<int> GetTotalLikes(Guid postId);
		Task<Like> AddLikeForBlogAsync(Like like);
	}
}
