﻿namespace Chayka
{
    using System.Collections.Generic;
    using System.Linq;
    using QuickGraph.Algorithms.ShortestPath;

    public class ShortestPathGraph<T>
        : QuickGraphGraph<T>
    {
        private readonly FloydWarshallAllShortestPathAlgorithm<T, QuickGraphEdge> algorithm;

        public ShortestPathGraph(IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<T>> edges) 
            : base(vertices, edges)
        {
            this.algorithm = new FloydWarshallAllShortestPathAlgorithm<T, QuickGraphEdge>(this.Graph, edge => 1);
            this.algorithm.Compute();
        }

        public override bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path)
        {
            IEnumerable<QuickGraphEdge> qgPath;
            var hasPath = algorithm.TryGetPath(source, target, out qgPath);

            path = (qgPath ?? Enumerable.Empty<QuickGraphEdge>()).Select(edge => edge.WrappedEdge);
            return hasPath;
        }
    }
}