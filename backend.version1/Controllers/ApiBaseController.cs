using backend.version1.Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace backend.version1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public abstract class ApiBaseController<T, TDto> : ControllerBase where T : class
    {
        protected readonly IBaseService<T, TDto> _service;
        protected readonly ILogger _logger;

        public ApiBaseController(IBaseService<T, TDto> service, ILogger<ApiBaseController<T, TDto>> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}