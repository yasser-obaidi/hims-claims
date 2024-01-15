using Mapster;

namespace ClamManagement.Helper
{
    public static class Mapper
    {
        public static TDestination MapTo<TDestination>(this object source)
        {
            return source.Adapt<TDestination>();
        }

        public static IQueryable<TDestination> MapTo<TDestination>(this IQueryable source)
        {
            return source.ProjectToType<TDestination>();
        }
    }
}
