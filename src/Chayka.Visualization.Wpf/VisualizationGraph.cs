namespace Chayka.Visualization.Wpf
{
    using System.Collections.Generic;
    using System.Linq;

    internal class VisualizationVertex
    {
        public string Label { get; private set; }

        public VisualizationVertex(string label)
        {
            this.Label = label;
        }

        public override string ToString()
        {
            return this.Label;
        }

        protected bool Equals(VisualizationVertex other)
        {
            return string.Equals(Label, other.Label);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((VisualizationVertex) obj);
        }

        public override int GetHashCode()
        {
            return (Label != null ? Label.GetHashCode() : 0);
        }
    }

    internal class VisualizationEdge
        : QuickGraph.Edge<VisualizationVertex>
    {
        public VisualizationEdge(VisualizationVertex source, VisualizationVertex target)
            : base(source, target)
        {
        }
    }

    internal class VisualizationGraph
        : QuickGraph.BidirectionalGraph<VisualizationVertex, VisualizationEdge>
    {
        public VisualizationGraph()
        {
        }

        public VisualizationGraph(IEnumerable<VisualizationVertex> vertices, IEnumerable<VisualizationEdge> edges)
        {
            this.AddVertexRange(vertices);
            this.AddEdgeRange(edges);

        }
    }

    internal class GraphLayout
        : GraphSharp.Controls.GraphLayout<VisualizationVertex, VisualizationEdge, VisualizationGraph> 
    {
        
    }

    internal static class VisualizationGraphExtensions
    {
        public static VisualizationGraph ToVisualizationGraph<T>(this IGraph<T> graph)
        {
            var vertices = graph.Vertices.Select(vertex => new VisualizationVertex(vertex.Content.ToString()));
            var edges = graph.Edges.Select(edge =>
                new VisualizationEdge(
                    new VisualizationVertex(edge.Source.Content.ToString()),
                    new VisualizationVertex(edge.Target.Content.ToString())
                )
            );
            return new VisualizationGraph(vertices, edges);
        }
    }
}
