﻿using Blog.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        protected BlogDbContext()
        {
        }

		public DbSet<Post> Posts { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Comment> Comments { get; set; }
    }
}
