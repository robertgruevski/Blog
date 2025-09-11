using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            context.Tags.Add(new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            });
            context.SaveChanges();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public IActionResult List() => View(context.Tags.ToList());

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var tag = context.Tags.FirstOrDefault(x => x.Id == id);
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
        public IActionResult Edit(EditTagRequest editTagRequest)
        {
            var tag = context.Tags.FirstOrDefault(x => x.Id == editTagRequest.Id);

            if (tag is not null)
            {
                tag.Name = editTagRequest.Name;
                tag.DisplayName = editTagRequest.DisplayName;

                context.SaveChanges();

                return RedirectToAction(nameof(List));
            }

            return View(editTagRequest);
        }

        [HttpPost]
        public IActionResult Delete(EditTagRequest editTagRequest)
        {
            var tag = context.Tags.FirstOrDefault(x => x.Id == editTagRequest.Id);
            if (tag is not null)
            {
                context.Tags.Remove(tag);
                context.SaveChanges();

                return RedirectToAction(nameof(List));
            }
            return RedirectToAction(nameof(Edit), new { id = editTagRequest.Id });
        }
    }
}
