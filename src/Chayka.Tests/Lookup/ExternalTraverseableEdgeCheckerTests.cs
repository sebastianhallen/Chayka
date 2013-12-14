namespace Chayka.Tests.Lookup
{
    using Chayka.Lookup;
    using FakeItEasy;
    using NUnit.Framework;

    [TestFixture]
    public class ExternalTraverseableEdgeCheckerTests
    {
        [Test]
        public void Should_should_call_external_traversability_function_when_checking_if_edge_is_traverseable()
        {
            var isTraversableChecked = false;
            var edgeChecker = new ExternalTraverseableEdgeChecker<int>(_ =>
                {
                    isTraversableChecked = true;
                    return false;
                });

            edgeChecker.IsTraverseable(A.Fake<IEdge<IVertex<int>>>());

            Assert.That(isTraversableChecked);
        }
    }
}