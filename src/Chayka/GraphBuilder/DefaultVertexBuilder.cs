namespace Chayka.GraphBuilder
{
    using System;

    public class DefaultVertexBuilder
        : IVertexBuilder
    {
        public IVertex<T> Build<T>(T content, Action onEntry)
        {
            return new DefaultVertex<T>(content, onEntry);
        }
    }
}