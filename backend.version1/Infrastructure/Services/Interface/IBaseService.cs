namespace backend.version1.Infrastructure.Services.Interface
{
    public interface IBaseService<T, TDto> where T : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();

        Task<TDto> GetByIdAsync(int id);

        Task AddAsync(TDto dto);

        Task UpdateAsync(TDto dto);

        Task DeleteAsync(int id);
    }
}