using System.Diagnostics;
using Blog.Web.Models;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IPostRepository postRepository;

		public HomeController(ILogger<HomeController> logger, IPostRepository postRepository)
        {
            _logger = logger;
			this.postRepository = postRepository;
		}

        public async Task<IActionResult> Index() => View(await postRepository.GetAllAsync());

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
