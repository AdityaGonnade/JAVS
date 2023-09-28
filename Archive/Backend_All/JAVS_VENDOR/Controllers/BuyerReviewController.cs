using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JAVS_VENDOR.REVIEW.REVIEWDataAccess;

using JAVS_VENDOR.REVIEW.ReviewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JAVS_VENDOR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuyerReviewController : Controller
    {
        private readonly ReviewDataAccess dataAccess;

        public BuyerReviewController(ReviewDataAccess rev)
        {
            dataAccess = rev;
        }

        [HttpGet]

        public async Task<List<Review>> GetAll()
        {
            return await dataAccess.GetAllReviews();
        }

        [HttpGet("{productname}")]

        public async Task<ProductReview> GetProductReview( string productname)
        {
            return await dataAccess.GetReviewForProduct(productname);
        }

        [HttpPost]

        public async Task<Review> AddReview([FromBody] AddReviewDTO obj)
        {
            return await dataAccess.AddReview(obj);
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteReview([FromBody] DeleteReviewDTO obj)
        {
             await dataAccess.Deletereview(obj);
            return Ok("Done");
        }


        [HttpPut]

        public async Task<Review> EditReview([FromBody] EditReviewDTO obj)
        {
            return await dataAccess.EditReview(obj);
        }




    }
}

