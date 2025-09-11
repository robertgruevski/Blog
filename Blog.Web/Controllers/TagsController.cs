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
    }
}
