using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Controllers
{
    public class TagsController : Controller
    {
        private readonly BlogDbContext context;
        public TagsController(BlogDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            await context.Tags.AddAsync(new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            });
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> List() => View(await context.Tags.ToListAsync());

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await context.Tags
                .FirstOrDefaultAsync(x => x.Id == id);

            if (tag is not null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = await context.Tags
                .FirstOrDefaultAsync(x => x.Id == editTagRequest.Id);

            if (tag is not null)
            {
                tag.Name = editTagRequest.Name;
                tag.DisplayName = editTagRequest.DisplayName;

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(List));
            }

            return View(editTagRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await context.Tags
                .FirstOrDefaultAsync(x => x.Id == editTagRequest.Id);
            if (tag is not null)
            {
                context.Tags.Remove(tag);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(List));
            }
            return RedirectToAction(nameof(Edit), new { id = editTagRequest.Id });
        }
    }
}
