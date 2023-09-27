using System;
namespace JAVS_VENDOR.ORDERS.OrderModels
{
	public class CancelOrderDTO
	{
        public string BuyerId { get; set; }

        public List<Guid> ItemIds { get; set; }

        public string OrderId { get; set; }
    }
}

