namespace JAVS_VENDOR.CART;

public class GetProductDto
{
    public string SellerId { get; set; }
    public string ProductName { get; set; }
    
    public string BuyerId { get; set; }
    
    public int quantity { get; set; }
}