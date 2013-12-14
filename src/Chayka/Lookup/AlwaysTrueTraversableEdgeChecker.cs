namespace Chayka.Lookup
{
    public class AlwaysTrueTraversableEdgeChecker<T>
        : ITraverseableEdgeChecker<T>
    {
        public bool IsTraverseable(IEdge<IVertex<T>> edge)
        {
            return true;
        }
    }
}