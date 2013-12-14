﻿namespace Chayka.GraphBuilder
{
    using Chayka.PathFinder.RandomWalk;
    using System.Collections.Generic;

    public class DefaultGraphBuilder<T>
        : IGraphBuilder<T>
    {
        private readonly IRandomWalkSessionFactory randomWalkSessionFactory;
        private readonly List<IVertex<T>> vertices;
        private readonly List<IEdge<IVertex<T>>> edges;

        public DefaultGraphBuilder() : this(new DefaultRandomWalkSessionFactory()){}

        public DefaultGraphBuilder(IRandomWalkSessionFactory randomWalkSessionFactory)
        {
            this.randomWalkSessionFactory = randomWalkSessionFactory;
            this.vertices = new List<IVertex<T>>();
            this.edges = new List<IEdge<IVertex<T>>>();
        }

        public IGraphBuilder<T> AddVertex(IVertex<T> vertex)
        {
            this.vertices.Add(vertex);
            return this;
        }

        public IGraphBuilder<T> AddEdge(IEdge<IVertex<T>> edge)
        {
            this.edges.Add(edge);
            return this;
        }

        public IGraph<T> Build()
        {
            return new DefaultGraph<T>(this.randomWalkSessionFactory, this.vertices, this.edges);
        }
    }
}