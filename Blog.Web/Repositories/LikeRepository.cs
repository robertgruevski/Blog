using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
	public class LikeRepository : ILikeRepository
	{
		private readonly BlogDbContext context;

		public LikeRepository(BlogDbContext context)
		{
			this.context = context;
		}

		public async Task<Like> AddLikeForBlogAsync(Like like)
		{
			await context.Likes.AddAsync(like);
			await context.SaveChangesAsync();
			return like;
		}

		public async Task<int> GetTotalLikes(Guid postId)
		{
			return await context.Likes.CountAsync(x => x.PostId == postId);
		}


	}
}
