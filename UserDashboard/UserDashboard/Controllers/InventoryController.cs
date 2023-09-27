using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using UserDashboard.Models.Domain;
using UserDashboard.Services;
using UserDashboard.Models.DTO;

namespace UserDashboard.Controllers;

//Dummy Controller for accessing Inventory Db and its APIs
[ApiController]
[Route("[controller]")]
public class InventoryController : Controller
{
    private readonly InventoryServices inventoryServices;

    public InventoryController(InventoryServices inventoryServices)
    {
        this.inventoryServices = inventoryServices;
    }

    //Fetching all the products
    [HttpGet]
    public async Task<List<Inventory>> GetAll() {

        return await inventoryServices.GetAllAsync();       
    }

    //creating DB Manually for testing
    [HttpPost]
    public async Task<IActionResult> CreateDB([FromBody] Inventory inv)
    {
        
        await inventoryServices.Create(inv);
        return CreatedAtAction(nameof(GetAll), new { id = inv.id}, inv);

    }
    
    //Adding a product to the Inventory DB
    [HttpPut]
    public async Task<IActionResult> AddProduct([FromBody] SellerInvDto SellerINV)
    {

        await inventoryServices.AddProduct(SellerINV);
        return Ok("Done");

    }
    
    //Searching a product in Inventory DB
    [HttpPost("SearchProduct")]
    public async Task<IActionResult> SearchProduct(SearchDto searchDto)
    {
       Console.WriteLine(searchDto.searchQuery );
        var result = await inventoryServices.SearchProduct(searchDto.searchQuery);
        
        return Ok(result);

    }
    
    //Return a specific product based on productName and sellerId
    [HttpPost]
    [Route("{productName}/{sellerId}")]
    public async Task<IActionResult> GetProductByProductNameAndSellerId( string productName, string sellerId)
    {
        try
        {
            var product = await inventoryServices.GetProductByProductNameAndSellerId(productName, sellerId);
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

