
using MongoDB;
using System;
using JAVS_VENDOR.INVENTORY;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using JAVS_VENDOR.CART.CartDataAccess;
using JAVS_VENDOR.INVENTORY;
using JWT_Token_Example.Order.OrderModels;

namespace JAVS_VENDOR.Repository;

public class OrderDataRepo:IOrderRepo
{
    private const string ConnectionString = "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";
    // add connection string here
    private const string DatabaseName = "SUJITH_DB";
    // add database name here
    private const string OrdersDB = "orders_new";


    private readonly IMongoCollection<Orders> ordersCollection;
    
    // private InventoryServices dataAccess;
    
    private CartDataRepo cartData;
    
    private InventoryDataRepo _inventoryDataRepo;

    public OrderDataRepo()
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

    public async Task<List<Orders>> GetOrdersVendor(string vendorId)
    {
        var filter = Builders<Orders>.Filter.Where(
            o => o.orders.Any(x => x.SellerId == vendorId && o.OrderStatus != "Cancelled")
        );
        
        var orders = await ordersCollection.Find(filter).ToListAsync();
        return orders;

    }

    public async Task<List<Orders>> GetOrdersPlacedBuyer(string id)
    {
        var filter = Builders<Orders>.Filter.Eq("BuyerId", id);
        var req = await ordersCollection.Find(filter).ToListAsync();

        return req;
    }

    public async Task Insert(Orders obj)
    {
        await ordersCollection.InsertOneAsync(obj);
    }

    public async Task<Orders> GetOrderById(string id)
    {
        var filter = Builders<Orders>.Filter.Eq("id", id);

        var update = Builders<Orders>.Update.Set("OrderStatus", "cancelled");

        var req = await ordersCollection.FindOneAndUpdateAsync(filter, update);
        return req;
    }
    
}