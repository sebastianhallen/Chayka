namespace Chayka
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Chayka.PathFinder;

    public interface IGraphWalker<in T>
    {
        void Walk(T souce, T target);
    }

    public class DefaultGraphWalker<T>
        : IGraphWalker<T>
    {
        private readonly IPathFinder<T> pathFinder;

        public DefaultGraphWalker(IPathFinder<T> pathFinder)
        {
            this.pathFinder = pathFinder;
        }

        public void Walk(T souce, T target)
        {
            var path = this.pathFinder.PathBetween(souce, target);

            foreach (var edge in path)
            {
                edge.OnTraverse();
            }
        }
    }
}
