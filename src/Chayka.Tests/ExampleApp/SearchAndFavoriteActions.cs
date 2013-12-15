namespace Chayka.Tests.ExampleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchAndFavoriteActions
        : ISearchAndFavoriteActions
    {
        private IEnumerable<Item> currentSearchResultsField;
        private Item currentItemField;
        private readonly IEnumerable<Item> store = new[]
            {
                new Item(ItemType.Clothing, "pants", "goes on your legs"),
                new Item(ItemType.Clothing, "t-shirt", "goes on your torso"),
                new Item(ItemType.Clothing, "socks", "goes on your feet"),

                new Item(ItemType.Furniture, "table", "something you can sit around"),
                new Item(ItemType.Furniture, "sofa", "seating for multiple people"),
                new Item(ItemType.Furniture, "chair", "seating for a single person"),

                
                new Item(ItemType.Vehicle, "car", "long range transportation for one to five people"),
                new Item(ItemType.Vehicle, "truck", "transports goods"),
                new Item(ItemType.Vehicle, "bike", "short range transport for single person"),
            };

        private AppState currentStateField;
        private AppState CurrentState
        {
            get { return this.currentStateField; }
            set
            {
                this.currentStateField = value;
            }
        }
        public IEnumerable<ItemType> Queries 
        {
            get { return this.store.Select(item => item.Type).Distinct(); }
        }

        public IEnumerable<Item> CurrentSearchResults
        {
            get
            {
                if (this.CurrentState != AppState.InSearchResults)
                {
                    throw new Exception("Not possible to get search results when not showing search results.");
                }
                return this.currentSearchResultsField;
            }
            set { this.currentSearchResultsField = value; }
        }

        public IEnumerable<Item> Favorites
        {
            get
            {
                if (!(this.CurrentState == AppState.InFavorites || this.CurrentState == AppState.InFavoriteItem))
                {
                    throw new Exception("Cannot list favorites when not in favorites");
                }

                return this.favorites;
            }
        }

        public bool IsCurrentItemFavorited
        {
            get { return this.favorites.Contains(this.CurrentItem); }
        }

        public bool HasFavorites
        {
            get { return this.favorites.Any(); }
        }

        private Item CurrentItem
        {
            get
            {
                if (!(this.CurrentState == AppState.InItem || this.CurrentState == AppState.InFavoriteItem))
                {
                    throw new Exception("Not viewing an item at the moment.");
                }
                return this.currentItemField;
            }
            set { this.currentItemField = value; }
        }

        private readonly IList<Item> favorites; 


        public SearchAndFavoriteActions()
        {
            this.CurrentState = AppState.InSearch;
            this.favorites = new List<Item>();
        }

        public void PerformSearch(ItemType type)
        {
            this.CurrentSearchResults = this.store.Where(item => item.Type.Equals(type));
            this.CurrentState = AppState.InSearchResults;
        }

        public void OpenItem(Item item)
        {
            this.CurrentItem = item;
            this.CurrentState = AppState.InItem;
        }

        public void OpenFavoriteItem(Item item)
        {
            this.CurrentItem = item;
            this.CurrentState = AppState.InFavoriteItem;
        }

        public void AddItemToFavorites()
        {
            if (this.CurrentState != AppState.InItem)
            {
                throw new Exception("Must be be viewing a single item to add it to favorites.");
            }
            if (this.favorites.Contains(this.CurrentItem))
            {
                throw new Exception("Cannot add an item to favorites twice");
            }

            this.favorites.Add(this.CurrentItem);
            
            this.CurrentState = AppState.InFavoriteItem;
        }

        public void RemoteItemFromFavorites()
        {
            if (this.CurrentState != AppState.InFavoriteItem)
            {
                throw new Exception("Cannot remove an item from favorites without being in the actual item.");
            }

            if (!this.favorites.Contains(this.CurrentItem))
            {
                throw new Exception("Cannot remove an item that is not favorited from favorites.");
            }

            this.CurrentState = AppState.InFavorites;
        }

        public void OpenFavorites()
        {
            this.CurrentState = AppState.InFavorites;
        }

        public void OpenSearch()
        {
            this.CurrentState = AppState.InSearch;
        }
    }

    public class Item
    {
        public ItemType Type { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        public Item(ItemType type, string title, string description)
        {
            this.Type = type;
            this.Title = title;
            this.Description = description;
        }
    }

    public enum ItemType
    {
        Clothing,
        Furniture,
        Vehicle
    }    

    public enum AppState
    {
        InSearchResults,
        InSearch,
        InItem,
        InFavoriteItem,
        InFavorites
    }

    public interface ISearchAndFavoriteActions
    {
        IEnumerable<ItemType> Queries { get; }
        IEnumerable<Item> CurrentSearchResults { get; }
        IEnumerable<Item> Favorites { get; }
        bool IsCurrentItemFavorited { get; }
        bool HasFavorites { get; }

        void PerformSearch(ItemType query);
        void OpenItem(Item item);
        void OpenFavoriteItem(Item item);

        void AddItemToFavorites();
        void RemoteItemFromFavorites();
        
        void OpenFavorites();
        void OpenSearch();

    }
}
