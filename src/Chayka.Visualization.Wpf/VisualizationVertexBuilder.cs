namespace Chayka.Visualization.Wpf
{
    using System;
    using Chayka.GraphBuilder;

    public class VisualizationVertexBuilder
        : IVertexBuilder
    {
        public IVertex<T> Build<T>(T content, Action onEntry)
        {
            return new VisualizingVertex<T>(content, () =>
            {
                onEntry();
                GraphVisualization.SetActiveVertex(content);
            });
        }

        private class VisualizingVertex<T>
            : DefaultVertex<T>
        {
            public VisualizingVertex(T content, Action onEntry)
                : base(content, () =>
                {
                    onEntry();
                    GraphVisualization.SetActiveVertex(content);
                })
            {
            }
        }
    }
}