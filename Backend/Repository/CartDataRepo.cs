using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using JAVS_VENDOR.INVENTORY;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Servers;
using  JAVS_VENDOR.Repository;
using JAVS_VENDOR.CART;
using JWT_Token_Example.Carts;
using MongoDB.Driver;

public class CartDataRepo:ICartRepo
{
    private const string ConnectionString = "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";

    // add connection string here
    private const string DatabaseName = "SUJITH_DB";

    // add database name here
    private const string cart = "cart";


    private InventoryServices dataAccess;
        
    public readonly IMongoCollection<Cart> cartCollection;
        
    public CartDataRepo(InventoryServices services)
    {
            
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        cartCollection = db.GetCollection<Cart>(cart);
        dataAccess = services;
    }
    
    public async Task<List<Cart>> GetAllCart(string BuyerId)
    {
        var filter = Builders<Cart>.Filter.Eq("BuyerId", BuyerId);
        var req = await cartCollection.Find(filter).ToListAsync();
        return req;
    }

    public async Task UpdateCart(FilterDefinition<Cart> filter, UpdateDefinition<Cart> update)
    {
        await cartCollection.UpdateOneAsync(filter, update);
    }

    public async Task InsertIntoCart(Cart obj)
    {
        await cartCollection.InsertOneAsync(obj);
    }

    public async Task deleteCart(string BuyerId)
    {
        var filter = Builders<Cart>.Filter.Eq("BuyerId", BuyerId);
        await cartCollection.DeleteOneAsync((filter));
    }
}