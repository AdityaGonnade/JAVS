using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JWT_Token_Example.Models;

[BsonIgnoreExtraElements]
public class Cart
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    
    [BsonElement("userid")]
    public Guid UserId { get; set; }
    
    [BsonElement("items")]
    public List<CartItems>? Items { get; set; }
}