namespace Chayka
{
    using System.Collections.Generic;

    public interface IRandomWalkSessionFactory
    {
        IRandomWalkSession<T> Start<T>(IEnumerable<QuickGraph.IEdge<T>> edges);
    }
}