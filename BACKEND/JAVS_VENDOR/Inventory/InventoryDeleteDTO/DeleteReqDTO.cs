using System;
using JAVS_VENDOR.INVENTORY_DOMAIN;
using Microsoft.AspNetCore.Mvc;

namespace JAVS_VENDOR.Inventory.InventoryDeleteDTO
{
	public class DeleteReqDTO
	{
        public string ProductName { get; set; }


        public string SellerId { get; set; }

     

       

      

        public int prev_quantity { get; set; }

        



       
        public int new_quantity{ get; set; }

    }
}

