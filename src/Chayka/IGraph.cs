namespace Chayka
{
    using System.Collections.Generic;
    using Chayka.PathFinder;

    public interface IGraph<T>
    {
        IEnumerable<IEdge<IVertex<T>>> Edges { get; }
        IEnumerable<IVertex<T>> Vertices { get; }

        IPathFinder<T> CreatePathFinder(PathType pathType);
    }
}
