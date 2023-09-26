using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace UserDashboard.Models.Domain
{
    public class Inventory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public string ProductName { get; set; }

        public string category { get; set; }

        public int totalQuantity { get; set; }

        public List<Seller> sellers { get; set; }
    }

    public class Seller
    {
        public string sellerId { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
        public string dateUploaded { get; set; }
        public List<string> tags { get; set; }
        public string imagesURL { get; set; }
        public int Price { get; set; }
    }


}


