using System;
using JAVS_VENDOR.INVENTORY_DOMAIN;
using static MongoDB.Driver.WriteConcern;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JAVS_VENDOR.ORDERS.OrderModels
{
	public class Orders
	{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }

        public string BillingAddressId { get; set; }

        public string BuyerId { get; set; }

        public List<OrderItems> orders { get; set; }

        public long TotalAmount { get; set; }

        public long TotalQuantity { get; set; }


    }
}

