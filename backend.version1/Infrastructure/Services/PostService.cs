using backend.version1.Domain.AutoMapper;
using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;
using backend.version1.Domain.Interfaces;
using backend.version1.Infrastructure.Common.Paginate;
using backend.version1.Infrastructure.Services.Interface;

namespace backend.version1.Infrastructure.Services
{
    public class PostService : BaseService<Post, PostDto>, IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository, IAutoMapperBase<Post, PostDto> mapper) : base(postRepository, mapper)
        {
            _postRepository = postRepository;
        }

        public async Task<PagedResponse<List<Post>>> GetAllPost(PaginationFilter validFilter, IUriService uriService, string route)
        {
            var posts = await _postRepository.GetAllPost(validFilter, uriService, route);

            return posts;
        }
    }
}