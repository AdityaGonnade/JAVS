namespace UserDashboard.Models.DTO;

//DTO for fetching product details based on SellerId and ProductName
public class GetProductDto
{
    public string SellerId { get; set; }
    
    public string ProductName { get; set; }
}