namespace Chayka.GraphBuilder
{
    using System;

    public interface IVertexBuilder
    {
        IVertex<T> Build<T>(T content, Action onEntry);
    }
}