using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Blog.Web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext context;

        public TagRepository(BlogDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchQuery, string? sortBy, string? sortDirection)
        {
            var query = context.Tags.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
                query = query.Where(x =>
                    x.Name.Contains(searchQuery) ||
                    x.DisplayName.Contains(searchQuery));

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                    query = isDesc ?
                        query.OrderByDescending(x => x.Name) :
                        query.OrderBy(x => x.Name);

                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                    query = isDesc ?
                        query.OrderByDescending(x => x.DisplayName) :
                        query.OrderBy(x => x.DisplayName);

            }

            // Pagination

            return await query.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id) => await context.Tags.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Tag> AddAsync(Tag tag)
        {
            await context.Tags.AddAsync(tag);
            await context.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var existingTag = await context.Tags
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingTag is not null)
            {
                context.Tags.Remove(existingTag);
                await context.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await context.Tags
                .FirstOrDefaultAsync(x => x.Id == tag.Id);

            if (existingTag is not null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await context.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }
    }
}
