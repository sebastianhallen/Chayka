namespace Chayka
{
    using Chayka.PathFinder;

    public interface IGraphWalker<in T>
    {
        void Walk(T souce, T target, PathType pathType);
    }
}
