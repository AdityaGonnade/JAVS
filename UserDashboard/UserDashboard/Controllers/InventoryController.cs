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
}

