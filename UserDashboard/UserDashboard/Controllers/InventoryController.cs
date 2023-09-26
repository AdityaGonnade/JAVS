using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using UserDashboard.Models.Domain;
using UserDashboard.Services;
using UserDashboard.Models.DTO;

namespace UserDashboard.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : Controller
{
    private readonly InventoryServices inventoryServices;

    public InventoryController(InventoryServices inventoryServices)
    {
        this.inventoryServices = inventoryServices;
    }

    [HttpGet]
    public async Task<List<Inventory>> GetAll() {

        return await inventoryServices.GetAllAsync();       
    }

    [HttpPost]
    public async Task<IActionResult> CreateDB([FromBody] Inventory inv)
    {
        
        await inventoryServices.Create(inv);
        return CreatedAtAction(nameof(GetAll), new { id = inv.id}, inv);

    }

    [HttpPut]
    public async Task<IActionResult> AddProduct([FromBody] SellerInvDto SellerINV)
    {

        await inventoryServices.AddProduct(SellerINV);
        return Ok("Done");

    }
    [HttpPost("SearchProduct")]
    public async Task<IActionResult> SearchProduct(SearchDto searchDto)
    {
       Console.WriteLine(searchDto.searchQuery );
        var result = await inventoryServices.SearchProduct(searchDto.searchQuery);
        
        return Ok(result);

    }
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

