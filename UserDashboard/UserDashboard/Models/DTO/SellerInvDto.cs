using System;
namespace UserDashboard.Models.DTO
{
	public class SellerInvDto
	{
		public string sellerID { get; set; }
		public string productName { get; set; }
		public string category { get; set; }
		public int quantity { get; set; }
		public string tags { get; set; }
		public string imgURL { get; set; }
		public string DateUploaded { get; set; }
		public string description { get; set; }
	}
}
