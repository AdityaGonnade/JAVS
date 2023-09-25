//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
//using UserDashboard.Models.Domain;
//using UserDashboard.Services;
//using UserDashboard.Models.DTO;
using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.INVENTORY_DOMAIN;
using JAVS_VENDOR.Inventory.InventoryDeleteDTO;

namespace UserDashboard.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class VendorInventoryController : Controller
    {
        private readonly DataAccess dataAccess;
        public VendorInventoryController(DataAccess inventoryServices)
        {
            this.dataAccess = inventoryServices;
        }
        [HttpGet]

        public async Task<List<Product>> GetAll()
        {
            return await dataAccess.GetAllP();
        }

        [HttpGet("{id}")]
        public async Task<List<Items>> GetAll(string id)
        {
            return await dataAccess.GetAllProducts(id);
        }
      
        [HttpPut]
        public async Task<IActionResult> AddProduct([FromBody] Items SellerINV)
        {
            await dataAccess.AddItem(SellerINV);
            return Ok("Done");
        }

        [HttpDelete]

        //eleteProduct(string vendorId, Items i, int n)

        public async Task<IActionResult> DeleteProduct([FromBody]DeleteReqDTO obj)
        {
            await dataAccess.DeleteProduct(obj);
            return Ok("Done");
        }
    }
}
;