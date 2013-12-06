namespace Chayka.Tests
{
    using FakeItEasy;
    using NUnit.Framework;
    using Edge = QuickGraph.IEdge<object>;

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
            var edge = A.Fake<Edge>();
            A.CallTo(() => edge.Source).Returns(0);

            Edge _;
            this.CreateSession(1, new[] {edge}).TryGetNextEdge(0, out _);

            A.CallTo(() => this.randomizer.NextInt(A<int>._)).MustHaveHappened();
        }

        [Test]
        public void Should_not_try_to_find_edge_when_max_path_length_has_been_reached()
        {
            Edge _;
            var session = this.CreateSession(0, new Edge[] { });

            var couldFoundEdge = session.TryGetNextEdge(0, out _);

            Assert.That(couldFoundEdge, Is.False);
        }

        private IRandomWalkSession<object> CreateSession(int maxPathLength, params Edge[] edges)
        {
            return new DefaultRandomWalkSessionFactory(randomizer, maxPathLength).Start(edges);
        }
    }
}
