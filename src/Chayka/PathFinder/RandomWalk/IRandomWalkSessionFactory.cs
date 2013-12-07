namespace Chayka.PathFinder.RandomWalk
{
    using System.Collections.Generic;

    public interface IRandomWalkSessionFactory
    {
        IRandomWalkSession<T> Start<T>(IEnumerable<IEdge<IVertex<T>>> edges);
    }
}