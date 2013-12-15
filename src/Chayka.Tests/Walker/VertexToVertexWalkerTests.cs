namespace Chayka.Tests.Walker
{
    using Chayka.PathFinder;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class VertexToVertexWalkerTests
        : WalkerTests
    {
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
        public void Should_execute_OnEntry_for_each_visited_vertex()
        {
            var target = A.Fake<IVertex<int>>();
            var edge = A.Fake<IEdge<IVertex<int>>>();
            A.CallTo(() => edge.Target).Returns(target);
            A.CallTo(() => this.pathFinder.PathBetween(A<IVertex<int>>._, A<IVertex<int>>._)).Returns(new[] { edge });

            walker.WalkBetween(0, 2, PathType.Shortest);

            A.CallTo(() => target.OnEntry()).MustHaveHappened();
        }

    }
}