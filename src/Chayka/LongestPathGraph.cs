namespace Chayka
{
    using QuickGraph.Algorithms.ShortestPath;
    using System.Collections.Generic;
    using System.Linq;

    //well, at least some kind of approximation by 
    //  finding the shortest path between all nodes
    //  finding all paths to target: pivotCandidates
    //  finding all paths from source to pivotCandidate source
    //  must not visit nodes twice
    //  if no "longest" path found, return shortest path, if available
    public class LongestPathGraph<T>
        : QuickGraphGraph<T>
    {
        private IEnumerable<IEdge<T>>[] paths;

        public LongestPathGraph(IEnumerable<IVertex<T>> vertices, IEnumerable<IEdge<T>> edges) 
            : base(vertices, edges)
        {
            this.paths = this.GetPaths();
        }

        public override bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path)
        {
            if (Equals(source, target))
            {
                path = Enumerable.Empty<IEdge<T>>();
                return true;
            }

            var pivotCandidates = from p in this.paths
                                  where p.Last().Target.Equals(target)
                                  where !p.Any(edge => edge.Source.Equals(source))
                                  select p;

            var pathsFromSource = from p in this.paths
                                  where p.First().Source.Equals(source)
                                  select p;


            var pathsViaPivots = from fromSource in pathsFromSource
                                 from fromPivot in pivotCandidates
                                 where !fromSource.Any(edge => edge.Source.Equals(target))
                                 where fromPivot.First().Source.Equals(fromSource.Last().Target)
                                 let fromPivotSources = fromPivot.Select(p => p.Source)
                                 let fromSourceSources = fromSource.Select(p => p.Source)
                                 where !fromPivotSources.Intersect(fromSourceSources).Any()
                                 select fromSource.Concat(fromPivot);
            var orderedCombinedPaths = from p in pathsViaPivots
                                       orderby p.Count() descending
                                       select p;
            
            if (orderedCombinedPaths.Any())
            {
                path = orderedCombinedPaths.First();
                return true;
            }

            var shortestPath = this.paths.FirstOrDefault(p =>
                {
                    var shortest = p.ToArray();
                    return shortest.First().Source.Equals(source) &&
                           shortest.Last().Target.Equals(target);
                });
            if (shortestPath != null)
            {
                path = shortestPath;
                return true;
            }

            path = Enumerable.Empty<IEdge<T>>();
            return false;

        }

        private IEnumerable<IEdge<T>>[] GetPaths()
        {
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<T, QuickGraph.IEdge<T>>(this.Graph, edge => 1);
            algorithm.Compute();

            var foundPaths = new List<IEnumerable<IEdge<T>>>();
            foreach (var source in this.Graph.Vertices)
                foreach (var target in this.Graph.Vertices)
                {
                    IEnumerable<QuickGraph.IEdge<T>> path;
                    if (algorithm.TryGetPath(source, target, out path))
                    {
                        foundPaths.Add(path.Cast<QuickGraphEdge>().Select(edge => edge.WrappedEdge).ToArray());
                    }
                }

            return foundPaths.ToArray();
        }
    }
}