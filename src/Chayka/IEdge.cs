namespace Chayka
{
    public interface IEdge<out T>
    {
        T Source { get; }
        T Target { get; }
        double Weight { get; }
        void OnTraverse();
        bool IsWalkable();
    }
}