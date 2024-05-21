using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;
using backend.version1.Infrastructure.Common.Paginate;
using backend.version1.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace backend.version1.Controllers
{
    public class PostController : ApiBaseController<Post, PostDto>
    {
        private readonly IPostService _postService;
        private readonly IUriService _uriService;

        public PostController(IPostService postService, ILogger<ApiBaseController<Post, PostDto>> logger, IUriService uriService) : base(postService, logger)
        {
            _postService = postService;
            _uriService = uriService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPost([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var posts = await _postService.GetAllPost(filter, _uriService, route);
            return Ok(posts);
        }
    }
}