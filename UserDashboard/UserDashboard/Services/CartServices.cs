using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UserDashboard.Data;
using UserDashboard.Models.Domain;
using UserDashboard.Models.DTO;

namespace UserDashboard.Services;

public class CartServices
{
    public CartServices()
    {
        
    }
    public CartServices(IOptions<InventoryDBSettings> inventorySettings)
    {
        MongoClient client = new MongoClient(inventorySettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(inventorySettings.Value.DatabaseName);
        CartCollection = database.GetCollection<Cart>(inventorySettings.Value.CartCollectionName);

    }
  
    private readonly IMongoCollection<Cart> CartCollection;
    
    public  async Task<List<Cart>> GetAllAsync()
    {
        return await CartCollection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task Create(Cart cart)
    {
        await CartCollection.InsertOneAsync(cart);
        return; 
    }
    //addtocart
    public Cart AddItemToCart(string userId, CartDto cartItem)
    {
        // Find the user's cart or create a new one if it doesn't exist
        var filter = Builders<Cart>.Filter.Eq(c => c.UserID, userId);
        var update = Builders<Cart>.Update.Push<CartDto>(c => c.Items, cartItem);

        var options = new FindOneAndUpdateOptions<Cart>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };
        
        var updatedCart = CartCollection.FindOneAndUpdate(filter, update, options);
        return updatedCart;
    }
    //display om the basis of id
    
    //remove from cart
    //update cart
    //buyNow
    
}