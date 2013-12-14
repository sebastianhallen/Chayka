namespace Chayka
{
    using Chayka.PathFinder;

    public interface IGraphWalker<in T>
    {
        void WalkBetween(T souce, T target, PathType pathType);
        void RandomWalk(T startVertex, int steps);
    }
}
