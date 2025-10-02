using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            // Haal alle grocery list items op
            var allItems = _groceriesRepository.GetAll();
            FillService(allItems); // vult Product property

            // Groepeer op Product en tel aantal keer dat het voorkomt
            var grouped = allItems
                .GroupBy(item => item.Product)
                .Select(g => new 
                { 
                    Product = g.Key, 
                    NrOfSells = g.Count() 
                })
                .OrderByDescending(x => x.NrOfSells)
                .Take(topX)
                .ToList();

            // Zet in BestSellingProducts model inclusief ranking
            var bestSellingList = new List<BestSellingProducts>();
            int rank = 1;
            foreach (var item in grouped)
            {
                bestSellingList.Add(new BestSellingProducts(
                    productId: item.Product.Id,
                    name: item.Product.Name,
                    stock: item.Product.Stock,
                    nrOfSells: item.NrOfSells,
                    ranking: rank
                ));
                rank++;
            }

            return bestSellingList;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
