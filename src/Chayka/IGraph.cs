namespace Chayka
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGraph<T>
    {
        IEnumerable<IEdge<IVertex<T>>> Edges { get; }
        IEnumerable<IVertex<T>> Vertices { get; }
    }
}
