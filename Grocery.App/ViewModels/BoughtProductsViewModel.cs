using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class BoughtProductsViewModel : BaseViewModel
    {
        private readonly IBoughtProductsService _boughtProductsService;

        [ObservableProperty]
        private Product selectedProduct;

        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<BoughtProducts> BoughtProductsList { get; set; } = new();

        public BoughtProductsViewModel(IBoughtProductsService boughtProductsService, IProductService productService)
        {
            _boughtProductsService = boughtProductsService;

            // Vul Picker met alle producten
            foreach (var p in productService.GetAll())
                Products.Add(p);
        }

        partial void OnSelectedProductChanged(Product oldValue, Product newValue)
        {
            BoughtProductsList.Clear();
            if (newValue == null) return;

            var boughtProducts = _boughtProductsService.Get(newValue.Id);
            foreach (var bp in boughtProducts)
            {
                BoughtProductsList.Add(bp);
            }
        }
    }
}