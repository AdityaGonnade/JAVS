using JWT_Token_Example.Reviews.ReviewModels;
using MongoDB.Driver;

namespace JAVS_VENDOR.Repository;

public interface IReviewRepo
{
    public Task<Review> Add(Review obj);
   
    public Task<List<Review>> FetchAllReviews();
   
    public Task<List<Review>> FetchProductReview(string product);
    
    public Task DeleteReview(string BuyerId, string productname);

    public Task<Review> FetchproductReviewBuyer(string BuyerId, string ProductName,FilterDefinition<Review>combinefilter);
    public Task UpdateDetails(Review revobj, EditReviewDTO obj);

}