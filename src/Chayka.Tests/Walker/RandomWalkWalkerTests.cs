namespace Chayka.Tests.Walker
{
    using Chayka.Lookup;
    using Chayka.Walker;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class RandomWalkWalkerTests
        : WalkerTests
    {
        [Test]
        public void Should_use_randomizer_when_sorting_edges_in_a_random_walk()
        {
            A.CallTo(() => this.vertexFinder.Find(graph, 0)).Returns(A.Dummy<IVertex<int>>());
            A.CallTo(() => this.edgeFinder.FindEgesFrom(this.graph, A<IVertex<int>>._, this.edgeChecker)).Returns(new[] { A.Dummy<IEdge<IVertex<int>>>() });

            this.walker.RandomWalk(0, 2);

            A.CallTo(() => this.randomizer.NextInt(int.MaxValue)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void Should_begin_random_walk_in_start_vertex()
        {
            var startVertex = A.Fake<IVertex<int>>();
            var startEdge = A.Fake<IEdge<IVertex<int>>>();
            A.CallTo(() => this.vertexFinder.Find(this.graph, 100)).Returns(startVertex);
            A.CallTo(() => this.edgeFinder.FindEgesFrom(this.graph, startVertex, this.edgeChecker)).Returns(new[] { startEdge });

            this.walker.RandomWalk(100, 1);

            A.CallTo(() => this.edgeFinder.FindEgesFrom(this.graph, startVertex, this.edgeChecker)).MustHaveHappened();
        }

        [Test]
        public void Should_continue_random_walk_in_next_vertex()
        {
            var startVertex = A.Fake<IVertex<int>>();
            var nextVertex = A.Fake<IVertex<int>>();
            var startEdge = A.Fake<IEdge<IVertex<int>>>();
            var nextEdge = A.Fake<IEdge<IVertex<int>>>();
            
            A.CallTo(() => this.vertexFinder.Find(this.graph, 100)).Returns(startVertex);
            A.CallTo(() => this.edgeFinder.FindEgesFrom(this.graph, startVertex, this.edgeChecker)).Returns(new[] { startEdge });
            A.CallTo(() => this.edgeFinder.FindEgesFrom(this.graph, nextVertex, this.edgeChecker)).Returns(new[] { nextEdge });
            A.CallTo(() => startEdge.Target).Returns(nextVertex);

            this.walker.RandomWalk(100, 2);

            A.CallTo(() => nextEdge.OnTraverse()).MustHaveHappened();
        }

        [Test]
        public void Should_not_be_possible_to_do_a_random_walk_when_start_vertex_cannot_be_found()
        {
            A.CallTo(() => this.vertexFinder.Find(this.graph, 123)).Returns(null);

            var exception = Assert.Throws<GraphLookupException>(() => this.walker.RandomWalk(123, 123));

            Assert.That(exception.Message, Is.EqualTo("Could not find vertex: 123"));
        }

        [Test]
        public void Should_abort_walk_when_unable_to_walk_any_more_edges()
        {
            var exception = Assert.Throws<GraphTraversalException>(() => this.walker.RandomWalk(1, 1));

            Assert.That(exception.Message, Is.EqualTo("Got stuck on vertex: 0"));
        }
    }
}