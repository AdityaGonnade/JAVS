using JWT_Token_Example.Inventory.InventoryModels;
using JWT_Token_Example.Models.MongoDBSettings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace JWT_Token_Example.Repository;

public class SearchAccessRepo :ISearchAccessRepo
{

    public readonly IMongoCollection<Product> productsCollection;
    public SearchAccessRepo(IOptions<MongoDbSettings> MongoDatabaseSettings)
    {
        var client = new MongoClient(MongoDatabaseSettings.Value.ConnectionString);
        var db = client.GetDatabase(MongoDatabaseSettings.Value.DatabaseName);
        productsCollection = db.GetCollection<Product>(MongoDatabaseSettings.Value.InventoryCollectionName);
        
    }
    public async Task<List<Product>> GetIventoryData ()
    {
        List<Product> InventoryData= await productsCollection.Find(new BsonDocument()).ToListAsync();
        return InventoryData;
    }
}