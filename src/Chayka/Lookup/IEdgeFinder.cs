namespace Chayka.Lookup
{
    using System.Collections.Generic;

    public interface IEdgeFinder<T>
    {
        IEnumerable<IEdge<IVertex<T>>> FindEgesFrom(IGraph<T> graph, IVertex<T> vertex, ITraverseableEdgeChecker<T> traverseableEdgeChecker);
    }

    public interface ITraverseableEdgeChecker<in T>
    {
        bool IsTraverseable(IEdge<IVertex<T>> edge);
    }
}