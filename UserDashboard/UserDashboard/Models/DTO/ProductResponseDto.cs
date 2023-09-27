using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserDashboard.Models.DTO;

//DTO for Response structure from GetProduct Request
public class ProductResponseDto
{
    
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string ProductName { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string imagesURL { get; set; }
        
        public int Price { get; set; } 
        
        public string SellerId { get; set; } 
    
}