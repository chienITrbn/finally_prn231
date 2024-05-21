using backend.Infrastructure.Repositories;
using backend.version1.Domain.Entities;
using backend.version1.Domain.Interfaces;
using backend.version1.Helpers;
using backend.version1.Infrastructure.Common.Paginate;
using backend.version1.Infrastructure.Services.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.version1.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<PagedResponse<List<Post>>> GetAllPost(PaginationFilter validFilter, IUriService uriService, string route)
        {
            var totalRecords = await ApplicationDbContext.Posts.CountAsync();

            var posts = await ApplicationDbContext.Posts
                .Include(p => p.Comments)
                .Include(p => p.Reactions)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            return PaginationHelper.CreatePagedReponse(posts, validFilter, totalRecords, uriService, route);
        }
    }
}
