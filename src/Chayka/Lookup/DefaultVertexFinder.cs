namespace Chayka.Lookup
{
    using System.Linq;

    public class DefaultVertexFinder<T>
        : IVertexFinder<T>
    {
        public IVertex<T> Find(IGraph<T> graph, T vertex)
        {
            var matchingVertices = (from v in graph.Vertices
                                    where Equals(vertex, v.Content)
                                    select v).ToArray();
            if (!matchingVertices.Any())
            {
                throw GraphLookupException.VertexNotFound(vertex);
            }

            if (matchingVertices.Count() > 1)
            {
                throw GraphLookupException.MultipleVerticesFound(vertex);
            }

            return matchingVertices.Single();
        }
    }
}