namespace Chayka
{
    using QuickGraph.Algorithms;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RandomPathGraph<T>
        : QuickGraphGraph<T>
    {
        private Random random;

        public RandomPathGraph(IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<T>> edges) 
            : base(vertices, edges)
        {
            this.random = new Random();
        }

        public override bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path)
        {
            return this.TryCreatePath(source, target, out path);
            
        }

        private bool TryCreatePath(T source, T target, out IEnumerable<IEdge<T>> path)
        {
            var candidate = new List<QuickGraphEdge>();
            var pathFound = false;
            var maxPathLenght = 1000;

            var current = source;
            while (!Equals(current, target))
            {
                if (--maxPathLenght < 0) break;
                var edge = this.GetEdgesFor(current).FirstOrDefault();
                
                if (edge == null) break;
                
                current = edge.Target;
                candidate.Add(edge);

                if (Equals(current, target))
                {
                    pathFound = true;
                    break;
                }
            }
            path = candidate.Select(edge => edge.WrappedEdge);
            return pathFound;
        }

        private IOrderedEnumerable<QuickGraphEdge> GetEdgesFor(T vertex)
        {
            return from edge in this.Graph.Edges
                   where edge.Source.Equals(vertex)
                   orderby this.Random(12345)
                   select edge;
        }

        private int Random(int max)
        {
            return this.random.Next(max);
        }
    }
}