using System;
using System.ComponentModel;

namespace JAVS_VENDOR.REVIEW.ReviewModels
{
	public class EditReviewDTO
	{
        public string ProductName { get; set; }

        public string BuyerId { get; set; }


        [DefaultValue("unknown")]
        public string Description { get; set; }


         [DefaultValue(-1)]
        public long rating { get; set; }


        [DefaultValue("unknown")]
        public string ImageURL { get; set; }
    }
}

