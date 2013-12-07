namespace Chayka.PathFinder
{
    using System.Collections.Generic;

    public abstract class PathFinderBase<T>
        : IPathFinder<T>
    {
        public abstract bool TryGetPathBetween(IVertex<T> source, IVertex<T> target, out IEnumerable<IEdge<IVertex<T>>> path);

        public IEnumerable<IEdge<IVertex<T>>> PathBetween(IVertex<T> source, IVertex<T> target)
        {
            IEnumerable<IEdge<IVertex<T>>> path;
            if (!this.TryGetPathBetween(source, target, out path))
            {
                throw new NoPathFoundException(string.Format("Unable to find a path between {0} and {1}", source, target));
            }

            return path;
        }
    }
}