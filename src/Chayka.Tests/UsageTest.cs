namespace Chayka.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Chayka.GraphBuilder;
    using Chayka.Lookup;
    using Chayka.PathFinder;
    using Chayka.PathFinder.RandomWalk;
    using Chayka.Walker;
    using NUnit.Framework;

    [TestFixture]
    public class UsageTest
    {
        private IGraph<State> model;
        private IGraphWalker<State> walker;
        private enum State
        {
            Search,
            SingleItem,
            SearchResult,
            Favorites,
            FavoriteItem
        }

        [SetUp]
        public void Before()
        {
            this.model = new DefaultGraphBuilder<State>()
                .AddVertex(State.Search)
                .AddVertex(State.SingleItem)
                .AddVertex(State.SearchResult)
                .AddVertex(State.Favorites)
                .AddVertex(State.FavoriteItem)

                .AddEdge(State.SingleItem, State.Search, _ => Console.WriteLine("opening search"))
                .AddEdge(State.SingleItem, State.FavoriteItem, _ => Console.WriteLine("adding to favorites"))
                .AddEdge(State.SingleItem, State.Favorites, _ => Console.WriteLine("opening favorites"))
                .AddEdge(State.SearchResult, State.SingleItem, _ => Console.WriteLine("opening item"))
                .AddEdge(State.SearchResult, State.Favorites, _ => Console.WriteLine("opening favorites"))
                .AddEdge(State.SearchResult, State.Search, _ => Console.WriteLine("opening search"))
                .AddEdge(State.Search, State.SearchResult, _ => Console.WriteLine("performing search"))
                .AddEdge(State.Search, State.Favorites, _ => Console.WriteLine("opening favorites"))
                .AddEdge(State.Favorites, State.Search, _ => Console.WriteLine("opening search"))
                .AddEdge(State.Favorites, State.FavoriteItem, _ => Console.WriteLine("opening favorite item"))
                .AddEdge(State.FavoriteItem, State.Favorites, _ => Console.WriteLine("unfavoriting item"))
                .AddEdge(State.FavoriteItem, State.Favorites, _ => Console.WriteLine("opening favorites"))
                .AddEdge(State.FavoriteItem, State.Search, _ => Console.WriteLine("opening search"))
                
                .Build();

            this.walker = new DefaultGraphWalker<State>(this.model, new DefaultVertexFinder<State>(), new DefaultEdgeFinder<State>(), new DefaultRandomizer(1337));
        }

        [Test]
        public void Should_be_able_to_walk_between_search_and_favorite_item()
        {
            this.walker.WalkBetween(State.Search, State.FavoriteItem, PathType.Shortest);
        }

        [Test]
        public void Should_be_able_to_do_a_random_walk_with_a_fixed_number_of_steps()
        {
            this.walker.RandomWalk(State.Search, 100);
        }

    }
}
