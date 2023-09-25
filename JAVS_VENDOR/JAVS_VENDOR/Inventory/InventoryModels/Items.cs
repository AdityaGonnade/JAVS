using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JAVS_VENDOR.INVENTORY_DOMAIN
{
	public class Items
	{
       
        

        public string ProductName { get; set; }

        
        public string SellerId { get; set; }

        public string Category { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Descriptions { get; set; }

        public int quantity { get; set; }

        public int Discount { get; set; }

        public int Price { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateUploaded { get; set; }



        public string Status { get; set; }



        //public Items(Items m )
        //{
        //    this.ProductName = m.ProductName;
        //    this.quantity = m.quantity;
        //    this.SellerId = m.SellerId;
        //    this.Category = m.Category;
        //    this.Tags = m.Tags;
        //    this.Descriptions = m.Descriptions;
        //    this.Discount = m.Discount;
        //    this.Price = m.Price;
        //    this.ImageUrl = m.ImageUrl;
        //    this.DateUploaded = m.DateUploaded;
            

        //}
    }
}

