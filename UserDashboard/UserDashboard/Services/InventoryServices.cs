using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UserDashboard.Data;
using UserDashboard.Models.Domain;
using UserDashboard.Models.DTO;

namespace UserDashboard.Services
{
	public class InventoryServices
	{
		public InventoryServices()
		{

		}

        private readonly IMongoCollection<Inventory> InventoryCollection;


        public InventoryServices(IOptions<InventoryDBSettings> inventorySettings)
        {
            MongoClient client = new MongoClient(inventorySettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(inventorySettings.Value.DatabaseName);
            InventoryCollection = database.GetCollection<Inventory>(inventorySettings.Value.InventoryCollectionName);
        }


        public  async Task<List<Inventory>> GetAllAsync()
        {
            return await InventoryCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task Create(Inventory Inv)
        {
            await InventoryCollection.InsertOneAsync(Inv);
            return; 
        }

        public async Task AddProduct(SellerInvDto SellerInv)
        {

            var filter = Builders<Inventory>.Filter.Eq("name", SellerInv.productName);
            var resultDoc = await InventoryCollection.Find(filter).ToListAsync();

            var sellerList = new Seller();
            sellerList.sellerId = SellerInv.sellerID;
            sellerList.quantity = SellerInv.quantity;
            sellerList.description = SellerInv.description;
            sellerList.dateUploaded = SellerInv.DateUploaded;
            sellerList.tags = SellerInv.tags;
            sellerList.imagesURL = SellerInv.imgURL;
            sellerList.Price = SellerInv.Price;

            var ListSell = new List<Seller>();
            ListSell.Add(sellerList);

            if (resultDoc.Count>0 )
            {
                var total_quantity = 0;
                foreach(var items in resultDoc)
                {
                    foreach(var sellerDetails in items.sellers)
                    {

                        ListSell.Add(sellerDetails);
                    }
                    total_quantity = items.totalQuantity+SellerInv.quantity;
                }

                var update_seller_list = Builders<Inventory>.Update.Set("sellers", ListSell);
                await InventoryCollection.UpdateOneAsync(filter, update_seller_list);


                var update_quantity = Builders<Inventory>.Update.Set("totalQuantity", total_quantity);
                await InventoryCollection.UpdateOneAsync(filter, update_quantity);
            }
            else
            {


                var Inv = new Inventory
                {
                    ProductName = SellerInv.productName,
                    category = SellerInv.category,
                    totalQuantity = 0,
                    sellers = ListSell,


                };
                await InventoryCollection.InsertOneAsync(Inv);
            }

            return;
        }
        public async Task SearchProduct(SearchDto searchDto)
        {

            List<string> keywords = ExtractKeywords(searchDto.searchQuery);
            
        }
        
        //Extracting Keywords from SearchQuery
        private List<string> ExtractKeywords(string input)
        {
            // split by spaces and remove duplicates
            return input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList();
        }
        
        //SearchProduct 
        public async Task<List<SearchResponseDto>> SearchProduct(string input)
        {
            
            List<string> keywords = ExtractKeywords(input);
            List<Inventory> inventoryData= await InventoryCollection.Find(new BsonDocument()).ToListAsync();
            Console.WriteLine(inventoryData[0].category);

            // Case-insensitive search in Inventory table
            var results = inventoryData
                .Where(item => item.totalQuantity > 0 &&
                               item.sellers.Any(seller => seller.quantity > 0 ) &&
                               (keywords.Any(keyword =>
                                    item.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                                keywords.Any(keyword =>
                                    item.sellers.Any(seller => seller.tags.Contains(keyword, StringComparer.OrdinalIgnoreCase)))))
                .SelectMany(item => item.sellers.Where(seller=>seller.quantity>0), (item, seller) => new SearchResponseDto
                {
                    id = item.id,
                    name = item.ProductName,
                    category = item.category,
                    description = seller.description,
                    imagesURL = seller.imagesURL,
                    Price = seller.quantity > 0 ? seller.Price : int.MaxValue,
                    SellerId = seller.sellerId
                })
                .OrderBy(item => item.Price) // Sort  by price
                    .ToList();
            
            //Console.WriteLine(results);
            return results;
        }
        public async Task<ProductResponseDto> GetProductByProductNameAndSellerId(string productName, string sellerId)
        {
            List<Inventory> inventoryData= await InventoryCollection.Find(new BsonDocument()).ToListAsync();

            // case-insensitive search in Inventory data
            var result = inventoryData
                .Where(item => item.totalQuantity > 0 &&
                               item.sellers.Any(seller => seller.quantity > 0) &&
                               item.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase) &&
                               item.sellers.Any(seller => seller.sellerId == sellerId))
                .SelectMany(item => item.sellers.Where(seller=> seller.quantity>0), (item, seller) => new ProductResponseDto()
                {
                    Id = item.id,
                    ProductName = item.ProductName,
                    Category = item.category,
                    Description = seller.description,
                    imagesURL = seller.imagesURL,
                    Price = seller.Price,
                    SellerId = seller.sellerId
                })
                .FirstOrDefault(); // Return the first match or null if no matches

            return result;
        }

    }
}


