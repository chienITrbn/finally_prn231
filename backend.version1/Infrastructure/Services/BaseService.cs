using backend.Domain.Interfaces;
using backend.version1.Domain.AutoMapper;
using backend.version1.Infrastructure.Services.Interface;

public abstract class BaseService<T, TDto> : IBaseService<T, TDto> where T : class
{
    protected readonly IRepositoryBase<T> _repository;
    protected readonly IAutoMapperBase<T, TDto> _mapper;

    public BaseService(IRepositoryBase<T> repository, IAutoMapperBase<T, TDto> mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(_mapper.Map);
    }

    public virtual async Task<TDto> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return _mapper.Map(entity);
    }

    public virtual async Task AddAsync(TDto dto)
    {
        var entity = _mapper.Map(dto);
        await _repository.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(TDto dto)
    {
        var entity = _mapper.Map(dto);
        await _repository.UpdateAsync(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}