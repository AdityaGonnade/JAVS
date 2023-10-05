using JWT_Token_Example.Inventory.InventorySearchAccess;
using JWT_Token_Example.UtilityService;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductFetchingInventoryController : ControllerBase
{
    private readonly ISearchAccess searchAccess;

    public ProductFetchingInventoryController(ISearchAccess DataAccess)
    {
        this.searchAccess = DataAccess;
    }



    //Add Seller Name also for SellerID using postgres(search, product,category)
    [HttpPost("SearchProduct")]
    public async Task<IActionResult> SearchProduct(InventorySearchDto searchDto)
    {
        //Console.WriteLine(searchDto.searchQuery);
        try
        {
            Console.WriteLine(searchDto.searchQuery);
            var result = await searchAccess.SearchProduct(searchDto.searchQuery);

            if (result == null)
            {
                // Edge case when no results are found
                return NotFound("No products found.");
            }
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Handling other unexpected exceptions
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, "An error occurred while processing your request.");
        }

    }
    [HttpPost]
    [Route("{productName}/{sellerId}")]
    public async Task<IActionResult> GetProductByProductNameAndSellerId(string productName, string sellerId)
    {

        try
        {
            var product = await searchAccess.GetProductByProductNameAndSellerId(productName, sellerId);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound($"Product with name '{productName}' and seller ID '{sellerId}' not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    [HttpPost]
    [Route("SearchProductByCategory")]
    public async Task<IActionResult> GetProductByCategory(InventorySearchDto inventorySearchDto)
    {

        try
        {
            var product = await searchAccess.GetProductBycategory(inventorySearchDto.searchQuery);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound($"Product with this category '{inventorySearchDto.searchQuery}'  not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}