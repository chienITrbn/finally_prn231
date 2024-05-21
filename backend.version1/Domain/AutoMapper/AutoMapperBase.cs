using AutoMapper;

namespace backend.version1.Domain.AutoMapper
{
    public class AutoMapperBase<TSource, TDestination> : IAutoMapperBase<TSource, TDestination>
    {
        private readonly IMapper _mapper;

        public AutoMapperBase(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public TSource Map(TDestination destination)
        {
            return _mapper.Map<TDestination, TSource>(destination);
        }
    }
}