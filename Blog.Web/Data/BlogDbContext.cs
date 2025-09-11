using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options)
        {
        }

        protected BlogDbContext()
        {
        }

		public DbSet<Post> BlogPosts { get; set; }
		public DbSet<Tag> Tags { get; set; }
	}
}
