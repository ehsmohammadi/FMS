namespace MITD.Fuel.Infrastructure.Service
{
    public abstract class MapperDtoToType<TSource, TTarget> : IMapperDtoToType
        where TSource : class
        where TTarget : class
    {

        public abstract void BeforMap(ref TSource source);

        public abstract TTarget Map(TSource source);

        public abstract void AfterMap(ref TTarget target, params object[] sources);

        public TTarget Resolve(TSource source, params object[] sources)
        {
           BeforMap(ref source);

           TTarget target= Map(source);

            AfterMap(ref target,source);

            return target;
        }
    }
}