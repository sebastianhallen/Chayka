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
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<T, QuickGraphEdge>(this.Graph, edge => 1);
            algorithm.Compute();

            this.paths = this.GetPaths(algorithm);
        }

        public override bool TryGetPathBetween(T source, T target, out IEnumerable<IEdge<T>> path)
        {
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
                                                         p.First().Source.Equals(source) &&
                                                         p.Last().Source.Equals(target));
            if (shortestPath == null)
            {
                path = Enumerable.Empty<IEdge<T>>();
                return false;
            }

            path = shortestPath;
            return true;

        }

        private IEnumerable<IEdge<T>>[] GetPaths(FloydWarshallAllShortestPathAlgorithm<T, QuickGraphEdge> algorithm)
        {
            var foundPaths = new List<IEnumerable<IEdge<T>>>();
            foreach (var source in this.Graph.Vertices)
                foreach (var target in this.Graph.Vertices)
                {
                    IEnumerable<QuickGraphEdge> path;
                    if (algorithm.TryGetPath(source, target, out path))
                    {
                        foundPaths.Add(path.Select(edge => edge.WrappedEdge).ToArray());
                    }
                }

            return foundPaths.ToArray();
        }
    }
}