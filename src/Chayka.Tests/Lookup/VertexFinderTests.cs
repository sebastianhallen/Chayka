namespace Chayka.Tests.Lookup
{
    using Chayka.Lookup;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class VertexFinderTests
    {
        [Test]
        public void Should_throw_graph_lookup_exception_when_unable_to_find_a_vertex()
        {
            var finder = new DefaultVertexFinder<int>();
            var graph = A.Fake<IGraph<int>>();
            
            var exception = Assert.Throws<GraphLookupException>(() => finder.Find(graph, 123));

            Assert.That(exception.Message, Is.EqualTo("Could not find vertex: 123"));
        }

        [Test]
        public void Should_throw_graph_lookup_exception_when_multiple_vertices_are_found()
        {
            var finder = new DefaultVertexFinder<int>();
            var graph = A.Fake<IGraph<int>>();
            var v0 = A.Fake<IVertex<int>>();
            var v1 = A.Fake<IVertex<int>>();
            A.CallTo(() => v0.Content).Returns(123);
            A.CallTo(() => v1.Content).Returns(123);
            A.CallTo(() => graph.Vertices).Returns(new[] {v0, v1});

            var exception = Assert.Throws<GraphLookupException>(() => finder.Find(graph, 123));

            Assert.That(exception.Message, Is.EqualTo("Multiple vertices found matching: 123"));
        }

        [Test]
        public void Should_find_matching_vertex()
        {
            var finder = new DefaultVertexFinder<int>();
            var graph = A.Fake<IGraph<int>>();
            var v0 = A.Fake<IVertex<int>>();
            var v1 = A.Fake<IVertex<int>>();
            A.CallTo(() => v0.Content).Returns(123);
            A.CallTo(() => v1.Content).Returns(321);
            A.CallTo(() => graph.Vertices).Returns(new[] { v0, v1 });

            var vertex = finder.Find(graph, 123);

            Assert.That(vertex, Is.EqualTo(v0));
        }
    }
}