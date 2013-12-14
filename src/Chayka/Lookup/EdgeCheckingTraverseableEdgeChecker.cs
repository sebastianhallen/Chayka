namespace Chayka.Lookup
{
    public class EdgeCheckingTraverseableEdgeChecker<T>
        : ITraverseableEdgeChecker<T>
    {
        public bool IsTraverseable(IEdge<IVertex<T>> edge)
        {
            return edge.IsWalkable();
        }
    }
}