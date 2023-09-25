using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using JAVS_VENDOR.INVENTORY_DOMAIN;

namespace JAVS_VENDOR.INVENTORY
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        private string id { get; set; }

        public string ProductName { get; set; }



        public string Category { get; set; }


        public List<Items> items { get; set; }

        public int Quantity { get; set; }


        public string Status { get; set; }

        //public void setQuantity(Product p)
        //{
        //    int count = 0;
        //    foreach(var x in p)
        //    {
        //        foreach(var i in p.items)
        //        {
        //            count += i.quantity;
        //        }
        //    }

        //    Quantity = count;


        //}

        
    }
}

