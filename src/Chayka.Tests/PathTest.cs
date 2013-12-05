namespace Chayka.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    public abstract class PathTest
    {
        protected static string PathToString<T>(IEnumerable<IEdge<T>> path)
        {
            var pathArray = path.ToArray();

            if (!pathArray.Any()) return "()";

            return
                pathArray.First().Source + " -> " +
                string.Join(" -> ", pathArray.Select(edge => edge.Target));
        }
    }
}