namespace Chayka.GraphBuilder
{
    using Chayka.PathFinder.RandomWalk;
    using System.Collections.Generic;

    public class DefaultGraphBuilder<T>
        : IGraphBuilder<T>
    {
        private readonly IRandomWalkSessionFactory randomWalkSessionFactory;
        private readonly List<IVertex<T>> vertices;
        private readonly List<IEdge<IVertex<T>>> edges;

        public DefaultGraphBuilder() : this(new DefaultRandomWalkSessionFactory()){}

        public DefaultGraphBuilder(IRandomWalkSessionFactory randomWalkSessionFactory)
        {
            this.randomWalkSessionFactory = randomWalkSessionFactory;
            this.vertices = new List<IVertex<T>>();
            this.edges = new List<IEdge<IVertex<T>>>();
        }

        public IEnumerable<IVertex<T>> Vertices
        {
            get { return this.vertices; }
        }

        public IGraphBuilder<T> AddVertex(IVertex<T> vertex)
        {
            if (vertex == null || Equals(null, vertex.Content))
            {
                throw GraphConstructionException.NullVertex;
            }

            if (this.vertices.Contains(vertex))
            {
                throw GraphConstructionException.NonUniqueVertex(vertex);
            }

            this.vertices.Add(vertex);
            return this;
        }

        public IGraphBuilder<T> AddEdge(IEdge<IVertex<T>> edge)
        {
            if (edge == null)
            {
                throw GraphConstructionException.NullEdge;
            }

            if (Equals(null, edge.Source))
            {
                throw GraphConstructionException.NullEdgeSource;
            }

            if (Equals(null, edge.Target))
            {
                throw GraphConstructionException.NullEdgeTarget;
            }

            this.edges.Add(edge);
            return this;
        }

        public IGraph<T> Build()
        {
            return new DefaultGraph<T>(this.randomWalkSessionFactory, this.vertices, this.edges);
        }
    }
}