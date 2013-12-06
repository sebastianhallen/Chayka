namespace Chayka
{
    using System.Collections.Generic;

    public class DefaultGraphBuilder<T>
        : IGraphBuilder<T>
    {
        private readonly IRandomWalkSessionFactory randomWalkSessionFactory;
        private readonly List<IVertex<T>> vertices;
        private readonly List<IEdge<T>> edges;

        public DefaultGraphBuilder() : this(new DefaultRandomWalkSessionFactory()){}

        public DefaultGraphBuilder(IRandomWalkSessionFactory randomWalkSessionFactory)
        {
            this.randomWalkSessionFactory = randomWalkSessionFactory;
            this.vertices = new List<IVertex<T>>();
            this.edges = new List<IEdge<T>>();
        }

        public IGraphBuilder<T> AddVertex(IVertex<T> vertex)
        {
            this.vertices.Add(vertex);
            return this;
        }

        public IGraphBuilder<T> AddEdge(IEdge<T> edge)
        {
            this.edges.Add(edge);
            return this;
        }

        public IPathFinder<T> CreatePathFinder(PathType pathType)
        {
            if (pathType.Equals(PathType.Shortest)) return new ShortestPathGraph<T>(this.vertices, this.edges);
            if (pathType.Equals(PathType.Longest)) return new LongestPathGraph<T>(this.vertices, this.edges);
            if (pathType.Equals(PathType.Random)) return new RandomPathGraph<T>(this.randomWalkSessionFactory, this.vertices, this.edges);
            return null;
        }
    }
}