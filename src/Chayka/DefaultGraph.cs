namespace Chayka
{
    using System;
    using System.Collections.Generic;
    using Chayka.PathFinder;
    using Chayka.PathFinder.LongestPath;
    using Chayka.PathFinder.RandomWalk;
    using Chayka.PathFinder.ShortestPath;

    public class DefaultGraph<T>
        : IGraph<T>
    {
        private readonly IRandomWalkSessionFactory randomWalkSessionFactory;
        public IEnumerable<IEdge<IVertex<T>>> Edges { get; private set; }
        public IEnumerable<IVertex<T>> Vertices { get; private set; }

        public DefaultGraph(IRandomWalkSessionFactory randomWalkSessionFactory, IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<IVertex<T>>> edges)
        {
            this.randomWalkSessionFactory = randomWalkSessionFactory;
            this.Vertices = vertices;
            this.Edges = edges;
        }

        public IPathFinder<T> CreatePathFinder(PathType pathType)
        {
            if (pathType.Equals(PathType.Shortest)) return new ShortestPathFinder<T>(this.Vertices, this.Edges);
            if (pathType.Equals(PathType.Longest)) return new LongestPathFinder<T>(this.Vertices, this.Edges);
            if (pathType.Equals(PathType.Random)) return new RandomWalkPathFinder<T>(this.randomWalkSessionFactory, this.Edges);
            
            throw new Exception("Unhandled path type: " + pathType);
        }
    }
}