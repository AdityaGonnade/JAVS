using JAVS_VENDOR.ORDERS.OrderDataAccess;
using JWT_Token_Example.Order.OrderModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JWT_Token_Example.Controllers;
[ApiController]
[Route("[controller]")]
    public class OrderBuyerController : Controller
    {

        private readonly OrderServices _services;
        public OrderBuyerController(OrderServices orderServices)
        {
            this._services = orderServices;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var req=  await _services.GetAllP();
                if (req == null)
                    return BadRequest("cannot find orders");

                return Ok(req);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
           
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetAllOrders(string id)
        {
            try
            {
                var req=  await _services.GetOrdersPlacedBuyer(id);
                if (req == null)
                    return BadRequest("cannot find orders placed by buyer");

                return Ok(req);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
            
        }

        [HttpPost]

        public async Task<IActionResult> PlaceOrder(OrdersDTO obj)
        {
            try
            {
                var req= await _services.PlaceOrderBuyer(obj);
                if (req != null)
                    return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
           
        }

        // [HttpDelete]
        //
        // public async Task CancelOrder(CancelOrderDTO obj)
        // {
        //    await _services.OrderCancelled(obj);
        //  
        // }






    }
