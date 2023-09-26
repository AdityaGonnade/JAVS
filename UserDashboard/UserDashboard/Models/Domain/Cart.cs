using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UserDashboard.Models.DTO;

namespace UserDashboard.Models.Domain;


    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserID { get; set; }
        public IEnumerable<CartDto> Items { get; set; }
    }

    public class Item
    {
        public string SellerId { get; set; }
        public string Quantity { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }
    }
