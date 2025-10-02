using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class BestSellingProductsViewModel : BaseViewModel
    {
        private readonly IGroceryListItemsService _groceryListItemsService;

        public ObservableCollection<BestSellingProducts> Products { get; set; } 
            = new ObservableCollection<BestSellingProducts>();

        public BestSellingProductsViewModel(IGroceryListItemsService groceryListItemsService)
        {
            _groceryListItemsService = groceryListItemsService;
            Load();
        }

        public override void Load()
        {
            Products.Clear();
            var items = _groceryListItemsService.GetBestSellingProducts();
            foreach (var item in items)
            {
                Products.Add(item);
            }
        }

        public override void OnAppearing()
        {
            Load();
        }

        public override void OnDisappearing()
        {
            Products.Clear();
        }
    }
}