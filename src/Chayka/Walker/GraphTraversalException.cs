namespace Chayka.Walker
{
    using System;

    public class GraphTraversalException
        : Exception
    {
        public static GraphTraversalException StuckOn<T>(IVertex<T> vertex)
        {
            return new GraphTraversalException("Got stuck on vertex: " + (vertex.Content.ToString()));
        }

        private GraphTraversalException(string message)
            : base(message)
        {
        }
    }
}