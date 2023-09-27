using MongoDB.Bson.Serialization.Attributes;

namespace JWT_Token_Example.Models;

public class CartItems
{
    [BsonElement("sellerid")]
    public string SellerId { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; }

    [BsonElement("productname")]
    public string ProductName { get; set; }

    [BsonElement("price")]
    public int Price { get; set; }

    [BsonElement("image")]
    public string Image { get; set; }
    
}