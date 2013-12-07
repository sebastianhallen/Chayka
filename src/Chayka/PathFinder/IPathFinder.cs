namespace Chayka.PathFinder
{
    using System.Collections.Generic;

    public interface IPathFinder<T>
    {
        IEnumerable<IEdge<IVertex<T>>> PathBetween(IVertex<T> source, IVertex<T> target);
        bool TryGetPathBetween(IVertex<T> source, IVertex<T> target, out IEnumerable<IEdge<IVertex<T>>> path);
    }

    public static class PathFinderExtensions
    {
        public static IEnumerable<IEdge<IVertex<T>>> PathBetween<T>(this IPathFinder<T> pathFinder, T source, T target)
        {
            return pathFinder.PathBetween(new DefaultVertex<T>(source), new DefaultVertex<T>(target));
        }

        public static bool TryGetPathBetween<T>(this IPathFinder<T> pathFinder, T source, T target, out IEnumerable<IEdge<IVertex<T>>> path)
        {
            return pathFinder.TryGetPathBetween(new DefaultVertex<T>(source), new DefaultVertex<T>(target), out path);
        }
    }
}