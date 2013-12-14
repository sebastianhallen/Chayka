namespace Chayka
{
    using System.Linq;
    using Chayka.PathFinder;
    using Chayka.PathFinder.RandomWalk;

    public class DefaultGraphWalker<T>
        : IGraphWalker<T>
    {
        private readonly IGraph<T> graph;
        private readonly IRandomizer randomizer;

        public DefaultGraphWalker(IGraph<T> graph, IRandomizer randomizer)
        {
            this.graph = graph;
            this.randomizer = randomizer;
        }

        public void WalkBetween(T souce, T target, PathType pathType)
        {
            var pathFinder = this.graph.CreatePathFinder(pathType);

            var path = pathFinder.PathBetween(souce, target);

            foreach (var edge in path)
            {
                edge.OnTraverse();
            }
        }

        public void RandomWalk(T startVertex, int steps)
        {
            var currentVertex = this.graph.Vertices.Single(vertex => Equals(startVertex, vertex.Content));
            for (var i = 0; i < steps; ++i)
            {
                var currentEdge = (from edge in this.graph.Edges
                                     where Equals(edge.Source, currentVertex)
                                     orderby this.randomizer.NextInt(int.MaxValue)
                                     select edge).First();
                currentEdge.OnTraverse();

                currentVertex = currentEdge.Target;
            }
        }
    }
}