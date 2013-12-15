namespace Chayka.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Chayka.GraphBuilder;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultGraphBuilderTests
    {
        [Test]
        public void Should_not_be_possible_to_add_the_same_vertex_twice()
        {
            var builder = new DefaultGraphBuilder<int>();
            builder.AddVertex(100);

            var exception = Assert.Throws<GraphConstructionException>(() => builder.AddVertex(100));

            Assert.That(exception.Message, Is.EqualTo("Cannot add a vertex more than once. Vertex already exists: 100"));
        }

        [Test]
        public void Should_not_be_possible_to_add_a_null_vertex()
        {
            var builder = new DefaultGraphBuilder<object>();

            var exception = Assert.Throws<GraphConstructionException>(() => builder.AddVertex(null));
        
            Assert.That(exception.Message, Is.EqualTo("Cannot add a null vertex to a graph."));
        }

        [Test]
        public void Should_not_be_possible_to_add_a_vertex_with_null_content()
        {
            var builder = new DefaultGraphBuilder<object>();
            var vertex = A.Fake<IVertex<object>>();
            A.CallTo(() => vertex.Content).Returns(null);

            var exception = Assert.Throws<GraphConstructionException>(() => builder.AddVertex(vertex));

            Assert.That(exception.Message, Is.EqualTo("Cannot add a null vertex to a graph."));
        }

        [Test]
        public void Should_not_be_possible_to_add_a_null_edge()
        {
            var builder = new DefaultGraphBuilder<int>();

            var exception = Assert.Throws<GraphConstructionException>(() => builder.AddEdge(null));

            Assert.That(exception.Message, Is.EqualTo("Cannot add a null edge to a graph."));
        }

        [Test]
        public void Should_not_be_possible_to_add_an_edge_with_null_source()
        {
            var builder = new DefaultGraphBuilder<object>();
            var edge = A.Fake<IEdge<IVertex<object>>>();
            A.CallTo(() => edge.Source).Returns(null);

            var exception = Assert.Throws<GraphConstructionException>(() => builder.AddEdge(edge));

            Assert.That(exception.Message, Is.EqualTo("Cannot add an edge without a source vertex."));
        }

        [Test]
        public void Should_not_be_possible_to_add_an_edge_with_null_target()
        {
            var builder = new DefaultGraphBuilder<object>();
            var edge = A.Fake<IEdge<IVertex<object>>>();
            A.CallTo(() => edge.Target).Returns(null);

            var exception = Assert.Throws<GraphConstructionException>(() => builder.AddEdge(edge));

            Assert.That(exception.Message, Is.EqualTo("Cannot add an edge without a target vertex."));
        }

        [Test]
        public void Must_add_source_vertex_before_adding_an_edge()
        {
            var builder = new DefaultGraphBuilder<object>();

            var exception = Assert.Throws<GraphConstructionException>(() => builder.AddEdge(new object(), new object(), () => {}));

            Assert.That(exception.Message, Is.EqualTo("Must add source and target vertices before adding an edge."));
        }

        [Test]
        public void Must_add_target_vertex_before_adding_an_edge()
        {
            var builder = new DefaultGraphBuilder<int>();
            builder.AddVertex(0);

            var exception = Assert.Throws<GraphConstructionException>(() => builder.AddEdge(0, 1, () => {}));

            Assert.That(exception.Message, Is.EqualTo("Must add source and target vertices before adding an edge."));
        }
    }
}
