using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JAVS_VENDOR.ORDERS.OrderDataAccess;
using JAVS_VENDOR.ORDERS.OrderModels;
using JAVS_VENDOR.INVENTORY_DOMAIN;
using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.ORDERS.OrderModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JAVS_VENDOR.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrderVendorController : Controller
    {
        private readonly OrderDataAccess dataAccess;
        public OrderVendorController(OrderDataAccess orderServices)
        {
            this.dataAccess = orderServices;
        }

        [HttpGet]

        public async Task<List<Orders>> GetAll()
        {
            return await dataAccess.GetAllP();
        }



     
        [HttpGet("{id}")]
        public async Task<List<VendorOrdersDTO>> GetAll(string id)
        {
            return await dataAccess.GetAllOrdersVendor(id);
        }




    }
}

