namespace Chayka.Lookup
{
    using System;

    public class GraphLookupException
        : Exception
    {
        public static GraphLookupException VertexNotFound<T>(T vertex)
        {
            return new GraphLookupException("Could not find vertex: " + (vertex.ToString()));
        }

        public static GraphLookupException MultipleVerticesFound<T>(T vertex)
        {
            return new GraphLookupException("Multiple vertices found matching: " + (vertex.ToString()));
        }

        private GraphLookupException(string message)
            : base(message)
        {
        }
    }
}