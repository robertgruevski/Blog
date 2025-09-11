using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
	public class TagsController : Controller
	{
		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
			var name = addTagRequest.Name;
			var displayName = addTagRequest.DisplayName;

            return View("Add");
        }
    }
}
