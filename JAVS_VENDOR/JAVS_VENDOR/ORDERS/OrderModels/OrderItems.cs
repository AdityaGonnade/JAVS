using System;
using JAVS_VENDOR.INVENTORY_DOMAIN;
using static MongoDB.Driver.WriteConcern;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JAVS_VENDOR.ORDERS.OrderModels
{
	public class OrderItems
	{


        public string ItemId { get; set; }

		public string SellerId { get; set; }


        public string Price { get; set; }

		public int itemquantity { get; set; }


		public DateTime dateOfArrival { get; set; }
		

        public string OrderStatus { get; set; }

    }
}

