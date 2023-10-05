using System;
using MongoDB.Driver;
using JWT_Token_Example.Reviews.ReviewModels;
using MongoDB.Bson;
namespace JAVS_VENDOR.Repository;

public class ReviewDataRepo:IReviewRepo
{
    private const string ConnectionString = "mongodb+srv://vishalmishra:Kunal8199@cluster0.hqijrs7.mongodb.net/?retryWrites=true&w=majority";

    private const string DatabaseName = "SUJITH_DB";
  
    private const string ReviewsDB = "reviews";


    private readonly IMongoCollection<Review> reviewsCollection;


    public ReviewDataRepo()
    {
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        reviewsCollection = db.GetCollection<Review>(ReviewsDB);
    }
    public async Task<Review> Add(Review obj)
    {
        await reviewsCollection.InsertOneAsync(obj);
        return obj;
    }

    public async Task<List<Review>> FetchAllReviews()
    {
        var req= await reviewsCollection.Find(new BsonDocument()).ToListAsync();
        return req;
        
    }

    public async Task<List<Review>> FetchProductReview(string product)
    {
        var filter = Builders<Review>.Filter.Eq("ProductName", product);
        return await reviewsCollection.Find(filter).ToListAsync();
    }

    public async Task DeleteReview(string BuyerId, string productname)
    {
        
        var namefilter = Builders<Review>.Filter.Eq("BuyerId", BuyerId);

        var prodfilter = Builders<Review>.Filter.Eq("ProductName", productname);

        var combinefilter = Builders<Review>.Filter.And(namefilter, prodfilter);

        await reviewsCollection.DeleteOneAsync(combinefilter);
    }

    public async Task<Review> FetchproductReviewBuyer(string BuyerId, string ProductName,FilterDefinition<Review> combinefilter)
    {
        var req = await reviewsCollection.Find((combinefilter)).ToListAsync();

        var revobj = req.First();
        
        return revobj;
    }

    public async Task UpdateDetails(Review revobj,EditReviewDTO obj)
    {
        var namefilter = Builders<Review>.Filter.Eq("BuyerId", obj.BuyerId);

        var prodfilter = Builders<Review>.Filter.Eq("ProductName", obj.ProductName);

        var combinefilter = Builders<Review>.Filter.And(namefilter, prodfilter);
        var updated_rating = obj.rating == -1 ? revobj.rating : obj.rating;

        var updated_imageUrl = obj.ImageURL == "unknown" ? revobj.ImageURL : obj.ImageURL;

        var updated_description = obj.Description == "unknown" ? revobj.Description : obj.Description;

        var update_rating = Builders<Review>.Update.Set("rating", updated_rating);

        var update_description = Builders<Review>.Update.Set("Description", updated_description);

        var update_ImageUrl = Builders<Review>.Update.Set("ImageURL", updated_imageUrl);
            
        var combinedUpdate = Builders<Review>.Update.Combine(update_rating, update_description, update_ImageUrl);


        await reviewsCollection.UpdateOneAsync(combinefilter, combinedUpdate);
        
    }
}