namespace Chayka.Lookup
{
    using System;

    public class ExternalTraverseableEdgeChecker<T>
        : ITraverseableEdgeChecker<T>
    {
        private readonly Func<IEdge<IVertex<T>>, bool> isTraverseable;

        public ExternalTraverseableEdgeChecker(Func<IEdge<IVertex<T>>, bool> isTraverseable)
        {
            this.isTraverseable = isTraverseable;
        }

        public bool IsTraverseable(IEdge<IVertex<T>> edge)
        {
            return this.isTraverseable(edge);
        }
    }
}