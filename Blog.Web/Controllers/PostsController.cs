using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public PostsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // Get tags from Repostiory
            var tags = await tagRepository.GetAllAsync();

            var model = new AddPostRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        public async Task<IActionResult> Add(AddPostRequest addPostRequest)
        {


            return RedirectToAction(nameof(Add));
        }
    }
}
