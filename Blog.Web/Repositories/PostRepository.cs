using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Repositories
{
    public class PostRepository : IPostRepository
    {

        private readonly BlogDbContext context;

        public PostRepository(BlogDbContext context)
        {
            this.context = context;
        }

        public async Task<Post> AddAsync(Post post)
        {
            await context.AddAsync(post);
            await context.SaveChangesAsync();
            return post;
        }

        public Task<Post?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await context.Posts.Include(x => x.Tags).ToListAsync();
        }

        public Task<Post?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Post?> UpdateAsync(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
