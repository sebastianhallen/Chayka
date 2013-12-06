namespace Chayka
{
    using System.Collections.Generic;
    using System.Linq;

    public class DefaultRandomWalkSession<T>
        : IRandomWalkSession<T>
    {
        private readonly int maxPathLength;
        private readonly IRandomizer randomizer;
        private readonly QuickGraph.IEdge<T>[] edges;

        private int currentPathLength;
        private const int MaxNumberOfOrderedEdges = 1 << 10;

        public DefaultRandomWalkSession(IEnumerable<QuickGraph.IEdge<T>> edges, IRandomizer randomizer, int maxPathLength)
        {
            this.edges = edges.ToArray();
            this.maxPathLength = maxPathLength;
            this.currentPathLength = 0;
            this.randomizer = randomizer;
        }

        public bool TryGetNextEdge(T @from, out QuickGraph.IEdge<T> edge)
        {
            if (++this.currentPathLength > this.maxPathLength)
            {
                edge = null;
                return false;
            }
            edge = (from e in this.edges
                    where e.Source.Equals(@from)
                    orderby this.randomizer.NextInt(MaxNumberOfOrderedEdges)
                    select e).FirstOrDefault();
            return edge != null;
        }
    }
}