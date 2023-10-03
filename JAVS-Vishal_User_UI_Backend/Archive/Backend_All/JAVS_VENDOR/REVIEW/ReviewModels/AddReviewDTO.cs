using System;
namespace JAVS_VENDOR.REVIEW.ReviewModels
{
	public class AddReviewDTO
	{
        public string ProductName { get; set; }

        public string BuyerId { get; set; }

        public string Description { get; set; }

        public long rating { get; set; }
        public string ImageURL { get; set; }
    }
}

