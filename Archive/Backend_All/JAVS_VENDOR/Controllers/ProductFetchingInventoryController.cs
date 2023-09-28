using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;

using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.Inventory;
using JAVS_VENDOR.Inventory.InventorySearchAccess;

namespace UserDashboard.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductFetchingProductController : Controller
{
    //private readonly DataAccess dataAccess;

    private readonly SearchAccess searchAccess;

    public ProductFetchingProductController(SearchAccess DataAccess)
    {
        this.searchAccess = DataAccess;
    }

    //[HttpGet]
    //public async Task<List<Product>> GetAll()
    //{

    //    return await searchAccess.GetAllP();
    //}


  
    [HttpPost("SearchProduct")]
    public async Task<IActionResult> SearchProduct(InventorySearchDto searchDto)
    {
        Console.WriteLine(searchDto.searchQuery);
        var result = await searchAccess.SearchProduct(searchDto.searchQuery);

        return Ok(result);

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
}

