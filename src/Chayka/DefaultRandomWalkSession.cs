namespace Chayka
{
    using System;
    using System.Linq;
    using QuickGraph;

    public class DefaultRandomWalkSession<T>
        : IRandomWalkSession<T>
    {
        private readonly IBidirectionalGraph<T, QuickGraph.IEdge<T>> graph;
        private readonly int maxPathLength;
        private readonly Random randomizer;
        private int currentPathLength;
        private const int MaxNumberOfOrderedEdges = 1 << 10;

        public DefaultRandomWalkSession(IBidirectionalGraph<T, QuickGraph.IEdge<T>> graph, int maxPathLength)
        {
            this.graph = graph;
            this.maxPathLength = maxPathLength;
            this.currentPathLength = 0;
            this.randomizer = new Random();
        }

        public bool TryGetNextEdge(T @from, out QuickGraph.IEdge<T> edge)
        {
            if (++this.currentPathLength >= this.maxPathLength)
            {
                edge = null;
                return false;
            }
            edge = (from e in this.graph.Edges
                    where e.Source.Equals(@from)
                    orderby this.NextRandom(MaxNumberOfOrderedEdges)
                    select e).FirstOrDefault();
            return edge != null;
        }

        private int NextRandom(int max)
        {
            return this.randomizer.Next(max);
        }
    }
}