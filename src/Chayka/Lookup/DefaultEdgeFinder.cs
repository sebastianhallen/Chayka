namespace Chayka.Lookup
{
    using System.Collections.Generic;
    using System.Linq;

    public class DefaultEdgeFinder<T>
        : IEdgeFinder<T>
    {
        public IEnumerable<IEdge<IVertex<T>>> FindEgesFrom(IGraph<T> graph, IVertex<T> vertex, ITraverseableEdgeChecker<T> traverseableEdgeChecker)
        {
            return from edge in graph.Edges
                   where VerticesMatch(edge.Source, vertex)
                   where traverseableEdgeChecker.IsTraverseable(edge)
                   select edge;
        }

        private static bool VerticesMatch(IVertex<T> vertex, IVertex<T> candidate)
        {
            return Equals(vertex, candidate);
        }

        private static bool VerticesMatch(IVertex<T> vertex, T candidate)
        {
            return Equals(vertex.Content, candidate);
        }
    }
}