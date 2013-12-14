namespace Chayka.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Chayka.Lookup;
    using FakeItEasy;

    public class EdgeCheckerFake
        : FakeConfigurator<ITraverseableEdgeChecker<int>>
    {
        public override void ConfigureFake(ITraverseableEdgeChecker<int> fakeObject)
        {
            A.CallTo(() => fakeObject.IsTraverseable(A<IEdge<IVertex<int>>>._)).Returns(true);
        }
    }
}
