namespace Chayka.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Chayka.PathFinder;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class WalkerTests
    {
        [UnderTest] private DefaultGraphWalker<int> walker;
        [Fake] private IPathFinder<int> pathFinder;
            
            
        [SetUp]
        public void Before()
        {
            Fake.InitializeFixture(this);
        }
            
        [Test]
        public void Should_execute_OnTraverse_action_on_each_edge_when_walking_edge()
        {
            var edge0 = A.Fake<IEdge<IVertex<int>>>();
            var edge1 = A.Fake<IEdge<IVertex<int>>>();
            A.CallTo(() => this.pathFinder.PathBetween(A<IVertex<int>>._, A<IVertex<int>>._)).Returns(new[] { edge0, edge1 });

            walker.Walk(0, 2);

            A.CallTo(() => edge0.OnTraverse()).MustHaveHappened();
            A.CallTo(() => edge1.OnTraverse()).MustHaveHappened();
        }

        [Test]
        public void Should_use_path_finder_to_get_next_edge()
        {
            this.walker.Walk(0, 1);

            A.CallTo(() => this.pathFinder.PathBetween(
                A<IVertex<int>>.That.Matches(v => v.Content.Equals(0)),
                A<IVertex<int>>.That.Matches(v => v.Content.Equals(1))
            )).MustHaveHappened();
        }
    }

}
