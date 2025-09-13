using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private readonly BlogDbContext context;

		public CommentRepository(BlogDbContext context)
		{
			this.context = context;
		}

		public async Task<Comment> AddAsync(Comment comment)
		{
			await context.Comments.AddAsync(comment);
			await context.SaveChangesAsync();
			return comment;
		}

		public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
		{
			return await context.Comments.Where(x => x.PostId == postId).ToListAsync();
		}
	}
}
