using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;

        public BoughtProductsService(
            IGroceryListItemsRepository groceryListItemsRepository,
            IGroceryListRepository groceryListRepository,
            IClientRepository clientRepository,
            IProductRepository productRepository)
        {
            _groceryListItemsRepository = groceryListItemsRepository;
            _groceryListRepository = groceryListRepository;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
        }

        public List<BoughtProducts> Get(int? productId)
        {
            var result = new List<BoughtProducts>();
            if (productId == null) return result;

            // Loop over alle boodschappenlijsten
            foreach (var list in _groceryListRepository.GetAll())
            {
                // Alle items in deze lijst
                var items = _groceryListItemsRepository.GetAllOnGroceryListId(list.Id);
                foreach (var item in items)
                {
                    if (item.ProductId == productId)
                    {
                        var client = _clientRepository.Get(list.ClientId);
                        var product = _productRepository.Get(productId.Value);
                        if (client != null && product != null)
                        {
                            result.Add(new BoughtProducts(client, list, product));
                        }
                    }
                }
            }

            return result;
        }
    }
}
