using Blog.Web.Migrations;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IPostRepository postRepository;
        private readonly ILikeRepository likeRepository;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
		private readonly ICommentRepository commentRepository;

		public PostsController(ITagRepository tagRepository, IPostRepository postRepository, ILikeRepository likeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ICommentRepository commentRepository)
        {
            this.tagRepository = tagRepository;
            this.postRepository = postRepository;
            this.likeRepository = likeRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
			this.commentRepository = commentRepository;
		}

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var post = await postRepository.GetByUrlHandleAsync(urlHandle);

            if (post is not null)
            {
                if (signInManager.IsSignedIn(User))
                {
                    var likesForPost = await likeRepository.GetLikesForBlog(post.Id);

                    var userId = userManager.GetUserId(User);

                    if (userId is not null)
                    {
                        var likesFromUser = likesForPost.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likesFromUser is not null;
                    }

                }

                var postCommentsDomainModel = await commentRepository.GetByPostIdAsync(post.Id);

                var postCommentsForView = new List<PostComment>();

                foreach (var comment in postCommentsDomainModel)
                {
                    postCommentsForView.Add(new PostComment
                    {
                        Description = comment.Description,
                        DateAdded = comment.DateAdded,
                        UserName = (await userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                    });
                }

                return View(new PostDetailsViewModel
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
                    Tags = post.Tags,
                    TotalLikes = await likeRepository.GetTotalLikes(post.Id),
                    Liked = liked,
                    Comments = postCommentsForView
                });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PostDetailsViewModel postDetailsViewModel)
        {
            if(signInManager.IsSignedIn(User))
            {
                var domainModel = new Comment
                {
                    PostId = postDetailsViewModel.Id,
                    Description = postDetailsViewModel.CommentDescription,
                    UserId = Guid.Parse(userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await commentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Posts", new { urlHandle = postDetailsViewModel.UrlHandle });
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var posts = await postRepository.GetAllAsync();

            return View(posts);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditPostRequest editPostRequest)
        {
            var post = new Post
            {
                Id = editPostRequest.Id,
                Heading = editPostRequest.Heading,
                PageTitle = editPostRequest.PageTitle,
                Content = editPostRequest.Content,
                ShortDescription = editPostRequest.ShortDescription,
                FeaturedImageUrl = editPostRequest.FeaturedImageUrl,
                UrlHandle = editPostRequest.UrlHandle,
                PublishedDate = editPostRequest.PublishedDate,
                Author = editPostRequest.Author,
                Visible = editPostRequest.Visible
            };

            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag is not null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }

            post.Tags = selectedTags;

            var updatedPost = await postRepository.UpdateAsync(post);
            if (updatedPost is not null)
            {
                return RedirectToAction(nameof(List));
            }

            return RedirectToAction(nameof(Edit));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(EditPostRequest editPostRequest)
        {
            var deletedPost = await postRepository.DeleteAsync(editPostRequest.Id);

            if (deletedPost is not null)
            {
                return RedirectToAction(nameof(List));
            }

            return RedirectToAction(nameof(Edit), new { id = editPostRequest.Id });
        }
    }
}
