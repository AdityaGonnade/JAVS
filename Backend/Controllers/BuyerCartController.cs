using JAVS_VENDOR.CART.CartDataAccess;
using JWT_Token_Example.Carts;
//using JWT_Token_Example.Carts.CartDataAccess;
using JWT_Token_Example.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace JWT_Token_Example.Controllers;

[ApiController]
[Route("[controller]")]
public class BuyerCartController : Controller
    {
        private readonly CartServices _services;

        public BuyerCartController(CartServices cartServices)
        {
            this._services = cartServices;
        }

        [HttpGet("id")]

        public async Task<IActionResult> GetAll(string id)
        {
            try
            {
                List<Cart> result = await _services.GetAllCartItems(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);

            }

            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");

                return StatusCode(500, "An internal server error occurred.");

            }


        }

        [HttpPost]

        public async Task<IActionResult> AddProductstoCart(GetProductDto obj)
        {
            try
            {
                var req = await _services.AddtoCart(obj);

                if (req == null)
                {
                    return BadRequest("cannot add to cart");
                }

                return Ok(req);

            }

            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");

                return StatusCode(500, "An internal server error occurred.");

            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTotalCart(string BuyerId)
        {
            try
            {
                await _services.DeleteCart(BuyerId);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");

                return StatusCode(500, "An internal server error occurred.");

            }


        }

        [HttpPut]

        public async Task<IActionResult> EditCart(EditCartDTO obj)
        {
            try
            {
                var req = await _services.EditItemsCart(obj);
                if (req == null)
                {
                    return BadRequest("cannot edit cart");
                }

                return Ok(req);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");

                return StatusCode(500, "An internal server error occurred.");

            }

        }
    }