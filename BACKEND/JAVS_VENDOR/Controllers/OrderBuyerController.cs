using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.ORDERS.OrderDataAccess;
using JAVS_VENDOR.ORDERS.OrderModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JAVS_VENDOR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderBuyerController : Controller
    {

        private readonly OrderDataAccess dataAccess;
        public OrderBuyerController(OrderDataAccess orderServices)
        {
            this.dataAccess = orderServices;
        }

        [HttpGet]

        public async Task<List<Orders>> GetAll()
        {
            return await dataAccess.GetAllP();
        }

        [HttpGet("id")]
        public async Task<List<Orders>> GetAllOrders(string id)
        {
            return await dataAccess.GetOrdersPlacedBuyer(id);
        }

        [HttpPost]

        public async Task<Orders> PlaceOrder(OrdersDTO obj)
        {
            return await dataAccess.PlaceOrderBuyer(obj);
        }

        [HttpDelete]

        public async Task<Orders> CancelOrders(CancelOrderDTO obj)

        {
            return await dataAccess.CancelOrder(obj);
        }






    }
}

