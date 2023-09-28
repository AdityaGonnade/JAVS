using System;
using JAVS_VENDOR.INVENTORY_DOMAIN;
using static MongoDB.Driver.WriteConcern;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JAVS_VENDOR.ORDERS.OrderModels
{
	public class OrderItems
	{
        

        public Guid ItemId { get; set; }

		public string SellerId { get; set; }


		public string ProductName { get; set; }


        public int Price { get; set; }

		public int itemquantity { get; set; }


		public DateTime dateOfArrival { get; set; }
		

        public string OrderStatus { get; set; }

    }
}

