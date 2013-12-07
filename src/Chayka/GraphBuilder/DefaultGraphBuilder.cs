namespace Chayka.GraphBuilder
{
    using System.Collections.Generic;
    using Chayka.PathFinder;
    using Chayka.PathFinder.LongestPath;
    using Chayka.PathFinder.RandomWalk;
    using Chayka.PathFinder.ShortestPath;

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

        public IGraphBuilder<T> AddVertex(IVertex<T> vertex)
        {
            this.vertices.Add(vertex);
            return this;
        }

        public IGraphBuilder<T> AddEdge(IEdge<IVertex<T>> edge)
        {
            this.edges.Add(edge);
            return this;
        }

        public IPathFinder<T> CreatePathFinder(PathType pathType)
        {
            if (pathType.Equals(PathType.Shortest)) return new ShortestPathFinder<T>(this.vertices, this.edges);
            if (pathType.Equals(PathType.Longest)) return new LongestPathFinder<T>(this.vertices, this.edges);
            if (pathType.Equals(PathType.Random)) return new RandomWalkPathFinder<T>(this.randomWalkSessionFactory, this.edges);
            return null;
        }
    }
}