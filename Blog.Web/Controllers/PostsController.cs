using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IPostRepository postRepository;

        public PostsController(ITagRepository tagRepository, IPostRepository postRepository)
        {
            this.tagRepository = tagRepository;
            this.postRepository = postRepository;
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

        [HttpPost]
        public async Task<IActionResult> Add(AddPostRequest addPostRequest)
        {
            var post = new Post
            {
                Heading = addPostRequest.Heading,
                PageTitle = addPostRequest.PageTitle,
                Content = addPostRequest.Content,
                ShortDescription = addPostRequest.ShortDescription,
                FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
                UrlHandle = addPostRequest.UrlHandle,
                PublishedDate = addPostRequest.PublishedDate,
                Author = addPostRequest.Author,
                Visible = addPostRequest.Visible
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addPostRequest.SelectedTags)
            {
                var existingTag = await tagRepository.GetAsync(Guid.Parse(selectedTagId));
                if (existingTag is not null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            post.Tags = selectedTags;

            await postRepository.AddAsync(post);

            return RedirectToAction(nameof(Add));
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var posts = await postRepository.GetAllAsync();

            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = await postRepository.GetAsync(id);
            var tagsDomainModel = await tagRepository.GetAllAsync();

            if (post is not null)
                return View(new EditPostRequest
                {
                    Id = post.Id,
                    Heading = post.Heading,
                    PageTitle = post.PageTitle,
                    Content = post.Content,
                    ShortDescription = post.ShortDescription,
                    FeaturedImageUrl = post.FeaturedImageUrl,
                    UrlHandle = post.UrlHandle,
                    PublishedDate = post.PublishedDate,
                    Author = post.Author,
                    Visible = post.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = post.Tags.Select(x => x.Id.ToString()).ToArray()
                });

            return View(null);
        }
    }
}
