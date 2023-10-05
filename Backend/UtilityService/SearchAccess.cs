using JWT_Token_Example.Inventory.InventoryModels;
using JWT_Token_Example.Models.MongoDBSettings;
using JWT_Token_Example.Repository;
using JWT_Token_Example.UtilityService;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JWT_Token_Example.Inventory.InventorySearchAccess;

public class SearchAccess : ISearchAccess
{
        

        private readonly ISearchAccessRepo _searchAccessRepo;

        public SearchAccess(ISearchAccessRepo searchAccessRepo)
        {
            
            _searchAccessRepo = searchAccessRepo;
        }
        

        private List<string> ExtractKeywords(string input)
        {
            // split by spaces and remove duplicates
            return input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();
        }

        public async Task<List<InventorySearchResponseDTO>> SearchProduct(string input)
        {
            List<string> keywords = ExtractKeywords(input);
            
            //fetch Inventory Data from Db using Repo!
            List<Product> inventoryData = await _searchAccessRepo.GetIventoryData();
            // Separate products that match ProductName exactly
            var exactMatchResults = inventoryData
              .Where(item => item.Quantity > 0 &&
                       item.items.Any(seller => seller.quantity > 0) &&
                       keywords.Any(keyword =>
                            item.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
              .SelectMany(item => item.items.Where(seller => seller.quantity > 0), (item, seller) => new InventorySearchResponseDTO
                { 
                    id = item.id, 
                    name = item.ProductName,
                    category = item.Category,
                    description = seller.Descriptions,
                    imagesURL = seller.ImageUrl,
                    Price = seller.quantity > 0 ? seller.Price : int.MaxValue,
                    SellerId = seller.SellerId
                }); 
            // Products that match keywords in seller's tags
             var tagMatchResults = inventoryData
              .Where(item => item.Quantity > 0 &&
                       item.items.Any(seller => seller.quantity > 0) &&
                       keywords.Any(keyword =>
                            item.items.Any(seller => seller.Tags.Contains(keyword, StringComparer.OrdinalIgnoreCase))))
             .SelectMany(item => item.items.Where(seller => seller.quantity > 0), (item, seller) => new InventorySearchResponseDTO
                {
                     id = item.id,
                     name = item.ProductName,
                     category = item.Category,
                     description = seller.Descriptions,
                     imagesURL = seller.ImageUrl,
                     Price = seller.quantity > 0 ? seller.Price : int.MaxValue,
                     SellerId = seller.SellerId
                 }); 
             // Concatenate the results, giving priority to exact matches
             var concatenatedResults = exactMatchResults.Concat(tagMatchResults);
             // Group by a unique key (e.g., id) and select the first item from each group
             var results = concatenatedResults
                .GroupBy(item => item.SellerId)
                .Select(group => group.First())
                .ToList();
             
             Console.WriteLine(results);
             return results;
        }
        
        
        public async Task<InventoryProductResponseDTO> GetProductByProductNameAndSellerId(string productName, string sellerId)
        { 
            
            List<Product> inventoryData = await _searchAccessRepo.GetIventoryData();

            //  case-insensitive search in Inventory data
            var result = inventoryData
                .Where(item => item.Quantity > 0 &&
                               item.items.Any(seller => seller.quantity > 0) &&
                               item.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase) &&
                               item.items.Any(seller => seller.SellerId == sellerId))
                .SelectMany(item => item.items.Where(seller => seller.quantity > 0), (item, seller) => new InventoryProductResponseDTO()
                {
                    Id = item.id,
                    ProductName = item.ProductName,
                    Category = item.Category,
                    Description = seller.Descriptions,
                    imagesURL = seller.ImageUrl,
                    Price = seller.Price,
                    SellerId = seller.SellerId
                }).ToList();
               // Return the first match or null if no matches

            
            foreach(var req in result)
            {
                if (req.SellerId == sellerId)
                    return req;
            }
            return null;
        }

        public async Task<List<InventoryProductResponseDTO>> GetProductBycategory(string category)
        {
            List<Product> inventoryData = await _searchAccessRepo.GetIventoryData();
            // Case-insensitive search in Inventory data based on category
            var results = inventoryData
                .Where(item => item.Quantity > 0 &&
                               item.Category.Equals(category, StringComparison.OrdinalIgnoreCase) &&
                               item.items.Any(seller => seller.quantity > 0))
                .SelectMany(item => item.items.Where(seller => seller.quantity > 0), (item, seller) => new InventoryProductResponseDTO()
                {
                    Id = item.id,
                    ProductName = item.ProductName,
                    Category = item.Category,
                    Description = seller.Descriptions,
                    imagesURL = seller.ImageUrl,
                    Price = seller.Price,
                    SellerId = seller.SellerId
                })
                .ToList();

            return results;
            
        }
}


