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
            InventoryCollection = database.GetCollection<Inventory>(inventorySettings.Value.CollectionName);
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
                    name = SellerInv.productName,
                    category = SellerInv.category,
                    totalQuantity = 0,

                    sellers = ListSell,


                };
                await InventoryCollection.InsertOneAsync(Inv);
            }

            return;
        }


    }
}


