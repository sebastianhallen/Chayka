namespace Chayka
{
    using Chayka.PathFinder;

    public class DefaultGraphWalker<T>
        : IGraphWalker<T>
    {
        private readonly IGraph<T> graph;

        public DefaultGraphWalker(IGraph<T> graph)
        {
            this.graph = graph;
        }

        public void Walk(T souce, T target, PathType pathType)
        {
            var pathFinder = this.graph.CreatePathFinder(pathType);

            var path = pathFinder.PathBetween(souce, target);

            foreach (var edge in path)
            {
                edge.OnTraverse();
            }
        }
    }
}