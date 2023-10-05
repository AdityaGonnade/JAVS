using JAVS_VENDOR.INVENTORY;
using MongoDB.Driver;
using JAVS_VENDOR.Repository;
using JWT_Token_Example.Inventory.InventoryModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

public class InventoryDataRepo:IInventoryRepo
{
    private const string ConnectionString = "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";

    private const string DatabaseName = "SUJITH_DB";
   
    private const string inventory = "inventory";

    public readonly IMongoCollection<Product> productsCollection;
    
    public InventoryDataRepo()
    {
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        productsCollection = db.GetCollection<Product>(inventory);
    }
  
    
    public async Task<List<Product>> FetchAllProducts()
    {
        var results = await productsCollection.Find(new BsonDocument()).ToListAsync();
        return results;
    }

    public async Task<List<Product>> FetchAllProductsVendor(FilterDefinition<Product> filter, ProjectionDefinition<Product> projection)
    {
        var products = await productsCollection.Find(filter).ToListAsync();
        return products;
    }

    

    public async Task<List<Product>> Fetchproduct(FilterDefinition<Product> filter)
    {
        var proItem=await productsCollection.Find(filter).ToListAsync();
        return proItem;
    }

    public async Task UpdateDB(FilterDefinition<Product> filter, UpdateDefinition<Product> update)
    {
        await productsCollection.UpdateOneAsync(filter, update);
    }

    public async Task InsertOneDB(Product p)
    {
        await productsCollection.InsertOneAsync(p);
    }

    public async Task ReplaceOneDB(FilterDefinition<Product> filter, Product p)
    {
        await productsCollection.ReplaceOneAsync( filter,p);
    }
    
    public async Task<Items> GetRequiredItem(string productname, string sellerid)
    {
        var pipeline = new BsonDocument[]
        {
            BsonDocument.Parse("{ $match: { ProductName: '" + productname + "' } }"),
            BsonDocument.Parse("{ $unwind: '$items' }"),
            BsonDocument.Parse("{ $match: { 'items.SellerId': '" + sellerid + "' } }"),
            BsonDocument.Parse("{ $replaceRoot: { newRoot: '$items' } }")
        };

        var aggregation = productsCollection.Aggregate<BsonDocument>(pipeline);

        var item = await aggregation.FirstOrDefaultAsync();
        Items Reqitem = new Items();

        if (item != null)
        {
            Reqitem= BsonSerializer.Deserialize<Items>(item);
        }

        return Reqitem;
    }
    
}