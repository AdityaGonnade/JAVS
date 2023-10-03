
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace JAVS_VENDOR.CART
{

    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        public string BuyerId { get; set; }


        public List<CartItems>? Items { get; set; }

    }
}