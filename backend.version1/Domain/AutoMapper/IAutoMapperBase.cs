namespace backend.version1.Domain.AutoMapper
{
    public interface IAutoMapperBase<TSource, TDestination>
    {
        TDestination Map(TSource source);

        TSource Map(TDestination destination);
    }
}