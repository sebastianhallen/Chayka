namespace Chayka.Tests
{
    using NUnit.Framework;
    using System.Linq;

    [TestFixture]
    public class ShortestPathGraphTests
    {
        private DefaultGraphBuilder<State> graphBuilder;
        
        [SetUp]
        public void Before()
        {
            this.graphBuilder = new DefaultGraphBuilder<State>();
            this.graphBuilder
                .AddVertex(new DefaultVertex<State>(State.A))
                .AddVertex(new DefaultVertex<State>(State.B))
                .AddVertex(new DefaultVertex<State>(State.C))
                .AddEdge(new DefaultEdge<State>(State.A, State.B))
                .AddEdge(new DefaultEdge<State>(State.B, State.C));
        }

        [Test]
        public void Should_be_able_to_find_shortest_path_in_graph()
        {
            var graph = this.graphBuilder.CreatePathFinder(PathType.Shortest);

            var pathAtoC = graph.PathBetween(State.A, State.C).ToArray();

            Assert.That(pathAtoC.First().Source, Is.EqualTo(State.A));
            Assert.That(pathAtoC.First().Target, Is.EqualTo(State.B));
            Assert.That(pathAtoC.Last().Source, Is.EqualTo(State.B));
            Assert.That(pathAtoC.Last().Target, Is.EqualTo(State.C));
        }

        [Test]
        public void Should_return_empty_path_when_unable_to_find_a_path()
        {
            var graph = this.graphBuilder.CreatePathFinder(PathType.Shortest);

            var pathCtoB = graph.PathBetween(State.C, State.B);

            Assert.That(pathCtoB.Any(), Is.False);
        }
    }
}
