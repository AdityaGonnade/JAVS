using System;
namespace JAVS_VENDOR.Inventory.InventoryUpdateDTO
{
	public class EditReqDTO
	{
        public string ProductName { get; set; }


        public string SellerId { get; set; }

       

        public List<string> Tags { get; set; }

        public List<string> Descriptions { get; set; }

        public int quantity { get; set; }

        public int Discount { get; set; }

        public int Price { get; set; }

        public string ImageUrl { get; set; }

        public DateTime DateUploaded { get; set; }


        public bool isQtyEdited { get; set; }


        public string Status { get; set; }
    }
}

