namespace Chayka
{
    using System.Collections.Generic;
    using System.Linq;

    public class RandomPathGraph<T>
        : QuickGraphGraph<T>
    {
        private readonly IRandomWalkSessionFactory sessionFactory;

        public RandomPathGraph(IRandomWalkSessionFactory sessionFactory, IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<T>> edges) 
            : base(vertices, edges)
        {
            this.sessionFactory = sessionFactory;
        }

        public override bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path)
        {
            var session = this.sessionFactory.Start(this.Graph.Edges);
            return this.TryCreatePath(session, source, target, out path);
            
        }

        private bool TryCreatePath(IRandomWalkSession<T> session, T source, T target, out IEnumerable<IEdge<T>> path)
        {
            var candidate = new List<QuickGraph.IEdge<T>>();
            var pathFound = false;

            var current = source;
            while (!Equals(current, target))
            {
                QuickGraph.IEdge<T> edge;
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
            path = candidate.Cast<QuickGraphEdge>().Select(edge => edge.WrappedEdge);
            return pathFound;
        }
    }
}