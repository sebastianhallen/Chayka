namespace Chayka.Lookup
{
    using System.Collections.Generic;

    public interface IEdgeFinder<T>
    {
        IEnumerable<IEdge<IVertex<T>>> FindEgesFrom(IGraph<T> graph, IVertex<T> vertex);
    }
}