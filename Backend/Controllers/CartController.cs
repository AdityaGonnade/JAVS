using JWT_Token_Example.Context;
using JWT_Token_Example.Models;
using JWT_Token_Example.Models.DTO;
using JWT_Token_Example.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace JWT_Token_Example.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CartController: ControllerBase , ICartService
{
    private readonly IMongoCollection<Cart> _cart;
    private readonly IMongoCollection<Product> _productCollection;
    
    public CartController(MongoDbContext _mongoDbContext, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(_mongoDbContext.DatabaseName);
        _cart = database.GetCollection<Cart>(_mongoDbContext.CollectionName);
        _productCollection = database.GetCollection<Product>(_mongoDbContext.CollectionName2);

    }
    
    [HttpPost("addtocart")]
    public async Task<IActionResult> AddtoCart(GetProductDto getProductDto ,Guid userId)
    {
        //fetch Data form Product database
        var filter = Builders<Product>.Filter.And(
            Builders<Product>.Filter.Eq("ProductName", getProductDto.ProductName),
            Builders<Product>.Filter.Eq("items.SellerId", getProductDto.SellerId)
        );
        var currentdata = await _productCollection.Find(filter).ToListAsync();
        var req = currentdata.First();
        int currentitemprice = 0;
        string currentitemimage="";
        
        foreach (var x in req.items)
        {
            if (x.SellerId == getProductDto.SellerId)
            {
                currentitemprice = x.Price;
                currentitemimage = x.ImageUrl;
                break;
            }
        }
        
        
        //chaeck if entry with current userid is present or not in Cart Db
        var currentItem = await _cart.Find(cart => cart.UserId == userId).FirstAsync();
        if (currentItem!=null)
        {
            CartItems cartItem = new CartItems()
            {
                SellerId = getProductDto.SellerId,
                Quantity = 1,
                ProductName = getProductDto.ProductName,
                Price = currentitemprice,
                Image = currentitemimage
            };

            currentItem.Items.Add(cartItem);

            _cart.DeleteOne(cart => cart.UserId == userId);
            _cart.InsertOne(currentItem);

        }
        else
        {
            //Create new cart object
            Cart cart = new Cart();
            cart.UserId = userId;

            CartItems cartItem = new CartItems()
            {
                SellerId = getProductDto.SellerId,
                Quantity = 1,
                ProductName = getProductDto.ProductName,
                Price = currentitemprice,
                Image = currentitemimage
            };
            cart.Items.Add(cartItem);
            
            _cart.InsertOne(cart);
        }

        return Ok(new
        {
            Message = "Added to Cart Successfully"
        });
    }
    
    [HttpDelete("deleteitem")]
    public async Task<IActionResult> RemoveFormCart(Guid userId, CartItems cartItem)
    {
        var currentItem = await _cart.Find(cart => cart.UserId == userId).FirstAsync();
        foreach (var x in currentItem.Items)
        {
            int index;
            if (x == cartItem)
            {
                currentItem.Items.Remove(x);
                break;
            }
        }

        return Ok(new
        {
            Message = "Item Removed form cart"
        });
    }

    [HttpGet("getcart")]
    public async Task<IEnumerable<CartItems>> GetCartItems(Guid userId)
    {
        var currentItem = await _cart.Find(cart => cart.UserId == userId).FirstAsync();
        return currentItem.Items.ToList();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> RemoveUser(Guid userId)
    {
        _cart.DeleteOne(cart => cart.UserId == userId);
        return Ok(new
        {
            Message = "Cart empty"
        });
    }

}