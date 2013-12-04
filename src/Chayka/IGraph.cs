namespace Chayka
{
    using System.Collections.Generic;

    public interface IGraph<T>
    {
        IEnumerable<IEdge<T>> PathBetween(T source, T target);
        bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path);
    }
}