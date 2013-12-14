namespace Chayka.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Chayka.PathFinder;
    using Chayka.PathFinder.RandomWalk;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class WalkerTests
    {
        [UnderTest] private DefaultGraphWalker<int> walker;
        [Fake] private IPathFinder<int> pathFinder;
        [Fake] private IGraph<int> graph;
        [Fake] private IRandomizer randomizer;

        [SetUp]
        public void Before()
        {
            Fake.InitializeFixture(this);
            A.CallTo(() => this.graph.CreatePathFinder(A<PathType>._)).Returns(this.pathFinder);
        }

        [Test]
        public void Should_execute_OnTraverse_action_on_each_edge_when_walking_edge()
        {
            var edge0 = A.Fake<IEdge<IVertex<int>>>();
            var edge1 = A.Fake<IEdge<IVertex<int>>>();
            A.CallTo(() => this.pathFinder.PathBetween(A<IVertex<int>>._, A<IVertex<int>>._)).Returns(new[] { edge0, edge1 });

            walker.WalkBetween(0, 2, PathType.Shortest);

            A.CallTo(() => edge0.OnTraverse()).MustHaveHappened();
            A.CallTo(() => edge1.OnTraverse()).MustHaveHappened();
        }

        [Test]
        public void Should_use_path_finder_to_get_next_edge()
        {
            this.walker.WalkBetween(0, 1, PathType.Shortest);

            A.CallTo(() => this.pathFinder.PathBetween(
                A<IVertex<int>>.That.Matches(v => v.Content.Equals(0)),
                A<IVertex<int>>.That.Matches(v => v.Content.Equals(1))
            )).MustHaveHappened();
        }

        [TestCase(PathType.Longest)]
        [TestCase(PathType.Random)]
        public void Should_construct_path_finder_using_the_path_type(PathType pathType)
        {
            this.walker.WalkBetween(0, 1, pathType);

            A.CallTo(() => this.graph.CreatePathFinder(pathType)).MustHaveHappened();
        }

        [Test]
        public void Should_use_randomizer_when_sorting_edges_in_a_random_walk()
        {
            IVertex<int> vertex = new DefaultVertex<int>(0);
            IEdge<IVertex<int>> edge = new DefaultEdge<IVertex<int>>(vertex, vertex, _ => { });
            A.CallTo(() => this.graph.Edges).Returns(new [] {edge});
            A.CallTo(() => this.graph.Vertices).Returns(new [] {vertex});

            this.walker.RandomWalk(0, 2);

            A.CallTo(() => this.randomizer.NextInt(int.MaxValue)).MustHaveHappened(Repeated.Exactly.Twice);
        }

        [Test]
        public void Should_begin_random_walk_in_start_vertex()
        {
            var vertex = A.Fake<IVertex<int>>();
            var startEdge = A.Fake<IEdge<IVertex<int>>>();
            A.CallTo(() => vertex.Content).Returns(100);
            A.CallTo(() => startEdge.Source).Returns(vertex);
            A.CallTo(() => this.graph.Vertices).Returns(new[] {vertex});
            A.CallTo(() => this.graph.Edges).Returns(new[] {startEdge});

            this.walker.RandomWalk(100, 1);

            A.CallTo(() => startEdge.OnTraverse()).MustHaveHappened();
        }

        [Test]
        public void Should_continue_random_walk_in_next_vertex()
        {
            var startVertex = A.Fake<IVertex<int>>();
            var nextVertex = A.Fake<IVertex<int>>();
            var startEdge = A.Fake<IEdge<IVertex<int>>>();
            var nextEdge = A.Fake<IEdge<IVertex<int>>>();
            A.CallTo(() => startVertex.Content).Returns(100);
            A.CallTo(() => nextVertex.Content).Returns(200);
            A.CallTo(() => startEdge.Source).Returns(startVertex);
            A.CallTo(() => startEdge.Target).Returns(nextVertex);
            A.CallTo(() => nextEdge.Source).Returns(nextVertex);

            A.CallTo(() => this.graph.Vertices).Returns(new[] { startVertex, nextVertex });
            A.CallTo(() => this.graph.Edges).Returns(new[] { startEdge, nextEdge });

            this.walker.RandomWalk(100, 2);

            A.CallTo(() => nextEdge.OnTraverse()).MustHaveHappened();
        }
    }
}
