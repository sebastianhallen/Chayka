namespace Chayka.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Chayka.GraphBuilder;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class GraphBuilderTests
    {
        private DefaultGraphBuilder<int> builder;

        [SetUp]
        public void Before()
        {
            this.builder = new DefaultGraphBuilder<int>();
        }

        [Test]
        public void Should_construct_graph_from_current_vertices()
        {
            var vertices = new[] {A.Dummy<IVertex<int>>(), A.Dummy<IVertex<int>>()}.ToList();
            vertices.ForEach(vertex => this.builder.AddVertex(vertex));

            var graph = this.builder.Build();

            Assert.That(graph.Vertices, Is.EquivalentTo(vertices));
        }

        [Test]
        public void Should_construct_graph_from_current_edges()
        {
            var edges = new[] {A.Dummy<IEdge<IVertex<int>>>(), A.Dummy<IEdge<IVertex<int>>>()}.ToList();
            edges.ForEach(edge => this.builder.AddEdge(edge));
           
            var graph = this.builder.Build();

            Assert.That(graph.Edges, Is.EquivalentTo(edges));
        }
    }
}