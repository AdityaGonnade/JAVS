using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JAVS_VENDOR.REVIEW.ReviewModels
{
	public class Review
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

		public string ProductName { get; set; }

		public string BuyerId { get; set; }

		public string Description { get; set; }

		public long rating { get; set; }

		public string ImageURL { get; set; }


		
	}
}

