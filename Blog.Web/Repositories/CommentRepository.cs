using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Repositories.Interfaces;

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
	}
}
