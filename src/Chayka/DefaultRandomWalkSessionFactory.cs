namespace Chayka
{
    using System.Collections.Generic;

    public class DefaultRandomWalkSessionFactory
        : IRandomWalkSessionFactory
    {
        private readonly IRandomizer randomizer;
        private readonly int maxPathLength;

        public DefaultRandomWalkSessionFactory()
            : this(new DefaultRandomizer(), 1000)
        {
        }

        public DefaultRandomWalkSessionFactory(IRandomizer randomizer, int maxPathLength)
        {
            this.randomizer = randomizer;
            this.maxPathLength = maxPathLength;
        }

        public IRandomWalkSession<T> Start<T>(IEnumerable<QuickGraph.IEdge<T>> edges)
        {
            return new DefaultRandomWalkSession<T>(edges, this.randomizer, this.maxPathLength);
        }
    }
}