using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserDashboard.Models.DTO;

//Dto for Search Response
public class SearchResponseDto
{
    
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public string SellerId { get; set; }

        public string name { get; set; }

        public string category { get; set; }
        
        public string description { get; set; }
        public string imagesURL { get; set; }
        public decimal Price { get; set; }
}