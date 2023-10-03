
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JAVS_VENDOR.CART;
using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.ORDERS.OrderDataAccess;
using JAVS_VENDOR.ORDERS.OrderModels;
using Microsoft.AspNetCore.Mvc;
using JAVS_VENDOR.CART.CartDataAccess;
namespace JAVS_VENDOR.Controllers{
    
[ApiController]
[Route("[controller]")]
public class BuyerCartController:Controller
{
    private readonly CartDataAccess dataAccess;
    public BuyerCartController(CartDataAccess cartServices)
    {
        this.dataAccess = cartServices;
    }
    
    [HttpGet("id")]

    public async Task<List<Cart>> GetAll(string id)
    {
        return await dataAccess.GetAllCartItems(id);
    }


    [HttpPost("mycart")]    
    public async Task<Cart> AddProductstoCart(GetProductDto obj)
    {
        var req= await dataAccess.AddtoCart(obj);
        return req;
    }
    
    [HttpDelete]
    public async Task DeleteTotalCart(string BuyerId)
    {
        await dataAccess.DeleteCart(BuyerId);
        
    }

    [HttpPut]

    public async Task<Cart> EditCart(EditCartDTO obj)
    {
        var req = await dataAccess.EditItemsCart(obj);
        return req;
    }
}
}