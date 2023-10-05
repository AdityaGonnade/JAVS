using JAVS_VENDOR.INVENTORY;
using JWT_Token_Example.Inventory.InventoryDeleteDTO;
using JWT_Token_Example.Inventory.InventoryEditDTO;
using JWT_Token_Example.Inventory.InventoryModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JWT_Token_Example.Controllers;
[ApiController]
[Route("[controller]")]
    public class VendorInventoryController : Controller
    {
        private readonly InventoryServices dataAccess;
        
        public VendorInventoryController(InventoryServices inventoryServices)
        {
            this.dataAccess = inventoryServices;
        }
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var req = await  dataAccess.GetAllP();
                if (req == null)
                    return BadRequest("cannot fetch products");
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
                var req =   await dataAccess.GetAllProducts(id);
                if (req == null)
                    return BadRequest("cannot fetch products");
                return Ok(req);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
           
        }
      
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody]Items SellerINV)
        {
            try
            {
                await dataAccess.AddItem(SellerINV);
           
                return Ok("Done");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
            
        }
        [HttpPut]
        public async Task<IActionResult> EditProduct([FromBody] EditReqDTO SellerINV)
        {
            try
            {
                await dataAccess.EditItem(SellerINV);
           
                return Ok("Done");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
            
          
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct([FromBody]DeleteReqDTO obj)
        {
            try
            {
                await dataAccess.DeleteProduct(obj);
           
                return Ok("Done");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while processing");
            
                return StatusCode(500, "An internal server error occurred.");
            
            }
       
           
        }
    }
