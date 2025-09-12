using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		[HttpGet]
		public IActionResult Index()
		{
			return Ok("This is the GET Images endpoint.");
		}
	}
}
