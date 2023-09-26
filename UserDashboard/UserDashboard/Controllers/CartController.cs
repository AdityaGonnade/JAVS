using Microsoft.AspNetCore.Mvc;
using UserDashboard.Models.Domain;
using UserDashboard.Models.DTO;
using UserDashboard.Services;

namespace UserDashboard.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController: Controller
{
    private readonly CartServices CartServices;

    //Injecting Service to the controller
    public CartController(CartServices cartServices)
    {
        this.CartServices = cartServices;
    }
    
    [HttpGet]
    public async Task<List<Cart>> GetAll() {

        return await CartServices.GetAllAsync();       
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCart([FromBody] Cart cart)
    {
        
        await CartServices.Create(cart);
        return CreatedAtAction(nameof(GetAll), new { id = cart.UserID}, cart);

    }
    [HttpPost("add")]
    public IActionResult AddToCart(string userId, [FromBody] CartDto cartItem)
    {
        var result = CartServices.AddItemToCart(userId, cartItem);
        if (result == null)
        {
            return NotFound("User not found or item not added to the cart.");
        }

        return Ok(result);
    }
   

   
}