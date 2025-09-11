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

        public async Task<Post?> DeleteAsync(Guid id)
        {
            var existingPost = await context.Posts.FindAsync(id);

            if(existingPost is not null)
            {
                context.Remove(existingPost);
                await context.SaveChangesAsync();

                return existingPost;
            }

            return null;
        }

        public async Task<IEnumerable<Post>> GetAllAsync() => await context.Posts.Include(x => x.Tags).ToListAsync();

        public async Task<Post?> GetAsync(Guid id) => await context.Posts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Post?> UpdateAsync(Post post)
        {
            var existingPost = await context.Posts
                .Include(x=>x.Tags)
                .FirstOrDefaultAsync(x => x.Id == post.Id);

            if(existingPost is not null)
            {
                existingPost.Id = post.Id;
                existingPost.Heading = post.Heading;
                existingPost.PageTitle = post.PageTitle;
                existingPost.Content = post.Content;
                existingPost.ShortDescription = post.ShortDescription;
                existingPost.FeaturedImageUrl = post.FeaturedImageUrl;
                existingPost.UrlHandle = post.UrlHandle;
                existingPost.PublishedDate = post.PublishedDate;
                existingPost.Author = post.Author;
                existingPost.Visible = post.Visible;
                existingPost.Tags = post.Tags;

                await context.SaveChangesAsync();
                return existingPost;
            }

            return null;
        }
    }
}
