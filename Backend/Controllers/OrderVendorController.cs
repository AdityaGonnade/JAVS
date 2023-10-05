using JAVS_VENDOR.ORDERS.OrderDataAccess;
using JWT_Token_Example.Order.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JWT_Token_Example.Controllers;
[ApiController]
[Route("[controller]")]
public class OrderVendorController : Controller
{
    private readonly OrderServices _services;
    public OrderVendorController(OrderServices orderServices)
    {
        this._services = orderServices;
    }

    [HttpGet]

    public async Task<IActionResult> GetAll()
    {
        try
        {
            var req= await _services.GetAllP();
            if (req == null)
                return BadRequest("cannot get orders");
            return Ok(req);

        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while processing");
            
            return StatusCode(500, "An internal server error occurred.");
            
        }
            
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAll(string id)
    {
        try
        {
            var req= await _services.GetAllOrdersVendor(id);
            if (req == null)
                return BadRequest("cannot get orders");
            return Ok(req);

        }
        catch (Exception ex)
        {
            Log.Error("An error occurred while processing");
            
            return StatusCode(500, "An internal server error occurred.");
            
        }
          
    }




}
