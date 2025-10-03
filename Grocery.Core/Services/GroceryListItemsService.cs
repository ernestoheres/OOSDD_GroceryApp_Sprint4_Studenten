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
            //pak producten bij id met een LINQ query en limiteer het tot top x
            var productsBySales = _groceriesRepository.GetAll()
            .GroupBy(i => i.ProductId)
            .Select(g => new { ProductId = g.Key, TotalSold = g.Sum(i => i.Amount) })
            .OrderByDescending(x => x.TotalSold)
            .ThenBy(x => x.ProductId)
            .Take(topX)
            .ToList();

            var sortedIds = productsBySales
            .Select(s => s.ProductId)
            .ToHashSet();

            var productsById = _productRepository.GetAll()
            .Where(p => sortedIds.Contains(p.Id))
            .ToDictionary(p => p.Id);

            // maak het een mooi lijstje voor de frontend
            var bestSellers = productsBySales
                .Select((s, index) =>
                {
                    var product = productsById.GetValueOrDefault(s.ProductId)
                               ?? new Product(0, "Onbekend", 0);

                    return new BestSellingProducts(
                        product.Id,
                        product.Name,
                        product.Stock,
                        s.TotalSold,
                        index + 1
                    );
                })
                .ToList();

            return bestSellers;

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
