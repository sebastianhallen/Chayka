namespace Chayka.Tests.PathFinder
{
    using Chayka.PathFinder.RandomWalk;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class DefaultRandomWalkSessionTests
    {
        private IRandomizer randomizer;

        [SetUp]
        public void Before()
        {
            this.randomizer = A.Fake<IRandomizer>();
        }

        [Test]
        public void Should_use_randomizer_when_sorting_edges()
        {
            var edge = A.Fake<IEdge<IVertex<object>>>();
            var vertex = A.Dummy<IVertex<object>>();
            A.CallTo(() => edge.Source).Returns(vertex);

            IEdge<IVertex<object>> _;
            this.CreateSession(1, new[] { edge }).TryGetNextEdge(vertex, out _);

            A.CallTo(() => this.randomizer.NextInt(A<int>._)).MustHaveHappened();
        }

        [Test]
        public void Should_not_try_to_find_edge_when_max_path_length_has_been_reached()
        {
            var edge = A.Fake<IEdge<IVertex<object>>>();
            var vertex = A.Dummy<IVertex<object>>();
            A.CallTo(() => edge.Source).Returns(vertex);

            IEdge<IVertex<object>> _;
            var couldFoundEdge = this.CreateSession(0, new[] { edge }).TryGetNextEdge(vertex, out _);

            Assert.That(couldFoundEdge, Is.False);
        }

        private IRandomWalkSession<object> CreateSession(int maxPathLength, params IEdge<IVertex<object>>[] edges)
        {
            return new DefaultRandomWalkSessionFactory(randomizer, maxPathLength).Start(edges);
        }
    }
}
