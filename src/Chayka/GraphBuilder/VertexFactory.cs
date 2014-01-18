namespace Chayka.GraphBuilder
{
    using System;

    public static class VertexFactory
    {
        private static IVertexBuilder _vertexBuilder;
        public static IVertexBuilder VertexBuilder
        {
            get { return _vertexBuilder ?? (_vertexBuilder = new DefaultVertexBuilder()); }
            set { _vertexBuilder = value; }
        }

        public static IVertex<T> Create<T>(T content, Action onEntry)
        {
            return VertexBuilder.Build(content, onEntry);
        }

        public static void Reset()
        {
            VertexBuilder = null;
        }
    }
}