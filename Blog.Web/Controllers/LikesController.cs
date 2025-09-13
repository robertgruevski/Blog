using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeRepository likeRepository;

        public LikesController(ILikeRepository likeRepository)
        {
            this.likeRepository = likeRepository;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddLikeRequest addLikeRequest)
        {
            if (addLikeRequest is not null)
            {
                await likeRepository.AddLikeForBlogAsync(
                    new Like
                    {
                        PostId = addLikeRequest.PostId,
                        UserId = addLikeRequest.UserId
                    });

                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("{postId:guid}/total-likes")]
        public async Task<IActionResult> GetTotalLikesForPost([FromRoute] Guid postId)
        {
            var totalLikes = await likeRepository.GetTotalLikes(postId);
            return Ok(totalLikes);
        }
    }
}
