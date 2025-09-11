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
	}
}
