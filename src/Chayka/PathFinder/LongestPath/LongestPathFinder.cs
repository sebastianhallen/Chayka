namespace Chayka.PathFinder.LongestPath
{
    using Chayka.GraphBuilder;
    using QuickGraph;
    using QuickGraph.Algorithms.ShortestPath;
    using System.Collections.Generic;
    using System.Linq;

    //well, at least some kind of approximation by 
    //  finding the shortest path between all nodes
    //  finding all paths to target: pivotCandidates
    //  finding all paths from source to pivotCandidate source
    //  must not visit nodes twice
    //  if no "longest" path found, return shortest path, if available
    public class LongestPathFinder<T>
        : PathFinderBase<T>
    {
        private readonly IEnumerable<Chayka.IEdge<IVertex<T>>>[] paths;
        private readonly IBidirectionalGraph<IVertex<T>, QuickGraphEdge<T>> graph;

        public LongestPathFinder(IEnumerable<IVertex<T>> vertices, IEnumerable<Chayka.IEdge<IVertex<T>>> edges)
        {
            this.graph = QuickGraphGraphBuilder<T>.Build(vertices, edges);
            this.paths = this.GetPaths();
        }

        public override bool TryGetPathBetween(IVertex<T> source, IVertex<T> target, out IEnumerable<Chayka.IEdge<IVertex<T>>> path)
        {
            if (Equals(source, target))
            {
                path = Enumerable.Empty<Chayka.IEdge<IVertex<T>>>();
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
                                       orderby p.Sum(e => e.Weight) descending
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

            path = Enumerable.Empty<Chayka.IEdge<IVertex<T>>>();
            return false;

        }

        private IEnumerable<Chayka.IEdge<IVertex<T>>>[] GetPaths()
        {
            var algorithm = new FloydWarshallAllShortestPathAlgorithm<IVertex<T>, QuickGraphEdge<T>>(this.graph, edge => edge.Weight);
            algorithm.Compute();

            var foundPaths = new List<IEnumerable<Chayka.IEdge<IVertex<T>>>>();
            foreach (var source in this.graph.Vertices)
                foreach (var target in this.graph.Vertices)
                {
                    IEnumerable<QuickGraphEdge<T>> path;
                    if (algorithm.TryGetPath(source, target, out path))
                    {
                        foundPaths.Add(path.Select(edge => edge.WrappedEdge).ToArray());
                    }
                }

            return foundPaths.ToArray();
        }
    }
}