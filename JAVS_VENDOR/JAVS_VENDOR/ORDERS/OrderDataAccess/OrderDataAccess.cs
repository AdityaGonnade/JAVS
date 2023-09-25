using System;
using JAVS_VENDOR.INVENTORY;
using MongoDB.Driver;
using JAVS_VENDOR.ORDERS.OrderModels;
using MongoDB.Bson;
using JAVS_VENDOR.INVENTORY_DOMAIN;

namespace JAVS_VENDOR.ORDERS.OrderDataAccess
{
	public class OrderDataAccess
	{

        private const string ConnectionString = "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";
        // add connection string here
        private const string DatabaseName = "SUJITH_DB";
        // add database name here
        private const string OrdersDB = "orders";


        private readonly IMongoCollection<Orders> ordersCollection;

      

        public OrderDataAccess()
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
           ordersCollection = db.GetCollection<Orders>(OrdersDB);
        }


        public async Task<List<Orders>> GetAllP()
        {

            var results = await ordersCollection.Find(new BsonDocument()).ToListAsync();
            return results;
        }

    

        public async Task<List<VendorOrdersDTO>> GetAllOrderItems(string vendorId)
        {

            var pr = await ordersCollection.Find(new BsonDocument()).ToListAsync();
            var result = new List<VendorOrdersDTO>();
            foreach (var order in pr)
            {
                foreach (var x in order.orders)
                {
                    if (x.SellerId == vendorId)
                    {


                        var req = new VendorOrdersDTO()
                        {
                            OrderId = order.OrderId,
                            BillingAddressId = order.BillingAddressId,
                            BuyerId = order.BuyerId,
                            orderitems = new List<VendorOrderItems>()
                            {
                                new VendorOrderItems(){
                                ItemId=x.ItemId,
                                SellerId=x.SellerId,
                                Price=x.Price,
                                itemquantity=x.itemquantity
                                }

                            }






                        };

                        result.Add(req);



                    }
                }

            }

          
            //var filter = Builders<Product>.Filter.Eq("items.SellerId", vendorId);
            //var results = await productsCollection.Find(filter).ToListAsync();
            return result;

        }
    }
}

