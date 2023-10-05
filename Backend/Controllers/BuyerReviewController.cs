using JAVS_VENDOR.REVIEW.REVIEWDataAccess;
using JWT_Token_Example.Reviews.ReviewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JWT_Token_Example.Controllers;

[ApiController]
[Route("[controller]")]
public class BuyerReviewController : Controller
    {
        private readonly ReviewServices _services;

        public BuyerReviewController(ReviewServices rev)
        {
            _services = rev;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var req= await _services.GetAllReviews();
                if (req == null)
                {
                    return BadRequest("cannot fetch reviews");
                }

                return Ok(req);

            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
            
        }

        [HttpGet("{productname}")]

        public async Task<IActionResult> GetProductReview( string productname)
        {
            try
            {   var req= await _services.GetReviewForProduct(productname);
                if (req == null)
                {
                    return BadRequest("cannot fetch reviews");
                }

                return Ok(req);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
         
        }

        [HttpPost]

        public async Task<IActionResult> AddReview([FromBody] AddReviewDTO obj)
        {
            try
            {
                var req=  await _services.AddReview(obj);
                if (req == null)
                    return BadRequest("cannot add review");

                return Ok(req);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            } 
           
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteReview([FromBody] DeleteReviewDTO obj)
        {
            try
            {
                await _services.Deletereview(obj);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            } 
          
        }


        [HttpPut]

        public async Task<IActionResult> EditReview([FromBody] EditReviewDTO obj)
        {
            try
            {
                await _services.EditReview(obj);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            } 
        }




    }