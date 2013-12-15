namespace Chayka.GraphBuilder
{
    using System;

    public class GraphConstructionException
        : Exception
    {
        private GraphConstructionException(string message)
            : base(message)
        {
            
        }

        public static GraphConstructionException NonUniqueVertex<T>(IVertex<T> vertex)
        {
            return new GraphConstructionException("Cannot add a vertex more than once. Vertex already exists: " + (vertex.ToString()));
        }


        public static Exception NullVertex
        {
            get
            {
                return new GraphConstructionException("Cannot add a null vertex to a graph.");
            }
        }

        public static Exception NullEdge
        {
            get
            {
                return new GraphConstructionException("Cannot add a null edge to a graph.");
            }
        }

        public static Exception NullEdgeSource
        {
            get
            {
                return new GraphConstructionException("Cannot add an edge without a source vertex.");
            }
        }

        public static Exception NullEdgeTarget
        {
            get
            {
                return new GraphConstructionException("Cannot add an edge without a target vertex.");
            }
        }

        public static Exception AddingEdgeBeforeVertex
        {
            get
            {
                return new GraphConstructionException("Must add source and target vertices before adding an edge.");
            }
        }
    }
}