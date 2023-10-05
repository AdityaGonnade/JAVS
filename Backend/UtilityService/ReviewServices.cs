using System;
using MongoDB.Driver;
using MongoDB.Bson;
using JAVS_VENDOR.Repository;
using JWT_Token_Example.Reviews.ReviewModels;

namespace JAVS_VENDOR.REVIEW.REVIEWDataAccess
{
	public class ReviewServices
	{
        private readonly IReviewRepo _reviewRepo;

        public ReviewServices(IReviewRepo repo)
        {

            _reviewRepo = repo;
        }

        public async Task<Review> AddReview(AddReviewDTO obj)
        {
            var review = new Review()
            {
                rating = obj.rating,
                BuyerId = obj.BuyerId,
                Description = obj.Description,
                ProductName = obj.ProductName,
                ImageURL =obj.ImageURL
            };

            await _reviewRepo.Add(review);
            return review;
          

        } 
        public async Task<List<Review>> GetAllReviews()
        {
            var req = await _reviewRepo.FetchAllReviews();
            return req;
        }

        public async Task<ProductReview> GetReviewForProduct(string product)
        {
            var filter = Builders<Review>.Filter.Eq("ProductName", product);
            var req = await _reviewRepo.FetchProductReview(product);

            var result = new List<ProductReviewDTO>();

            float avg = 0;
            int i = 0;
      
            foreach(var rev in req)
            {
                var prodRevDto = new ProductReviewDTO()
                {
                    BuyerId = rev.BuyerId,
                    Description = rev.Description,
                    ImageURL = rev.ImageURL
                };

                avg += rev.rating;
                i++;
                result.Add(prodRevDto);
            }


            avg /= i;

            var res = new ProductReview()
            {
                review = result,
                avgRating = avg
            };

            return res;
        }
        public async Task Deletereview(DeleteReviewDTO obj)
        {
            await _reviewRepo.DeleteReview(obj.BuyerId, obj.ProductName);
        }

        public async Task EditReview(EditReviewDTO obj)
        {
            var namefilter = Builders<Review>.Filter.Eq("BuyerId", obj.BuyerId);
            
            var prodfilter = Builders<Review>.Filter.Eq("ProductName", obj.ProductName);

            var combinefilter = Builders<Review>.Filter.And(namefilter, prodfilter);
            
            var req = await _reviewRepo.FetchproductReviewBuyer(obj.BuyerId, obj.ProductName,combinefilter);
            
            await _reviewRepo.UpdateDetails(req, obj);
        }
    }
}

