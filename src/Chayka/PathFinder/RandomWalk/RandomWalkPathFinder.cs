namespace Chayka.PathFinder.RandomWalk
{
    using System.Collections.Generic;
    using System.Linq;
    
    public class RandomWalkPathFinder<T>
        : PathFinderBase<T>
    {
        private readonly IRandomWalkSessionFactory sessionFactory;
        private readonly IEnumerable<IEdge<IVertex<T>>> edges;

        public RandomWalkPathFinder(IRandomWalkSessionFactory sessionFactory, IEnumerable<IEdge<IVertex<T>>> edges)
        {
            this.sessionFactory = sessionFactory;
            this.edges = edges;
        }

        public override bool TryGetPathBetween(IVertex<T> source, IVertex<T> target, out IEnumerable<IEdge<IVertex<T>>> path)
        {
            var session = this.sessionFactory.Start(this.edges);
            return this.TryCreatePath(session, source, target, out path);
            
        }

        private bool TryCreatePath(IRandomWalkSession<T> session, IVertex<T> source, IVertex<T> target, out IEnumerable<IEdge<IVertex<T>>> path)
        {
            if (Equals(source, target))
            {
                path = Enumerable.Empty<IEdge<IVertex<T>>>();
                return true;
            }

            var candidate = new List<IEdge<IVertex<T>>>();
            var pathFound = false;

            var current = source;
            while (!Equals(current, target))
            {
                IEdge<IVertex<T>> edge;
                if (!session.TryGetNextEdge(current, out edge))
                {
                    break;
                }

                current = edge.Target;
                candidate.Add(edge);

                if (Equals(current, target))
                {
                    pathFound = true;
                    break;
                }
            }
            path = candidate;
            return pathFound;
        }
    }
}