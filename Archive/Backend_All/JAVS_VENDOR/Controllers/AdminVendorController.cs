using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JAVS_VENDOR.PROFILE;
using JAVS_VENDOR.VENDORPROFILE_SQL_DATA;
using JAVS_VENDOR.VendorProfile.VendorProfileModels.VendorProfileDTO;
using JAVS_VENDOR.VendorProfileModels.VendorProfileModels.VendorProfileDomain;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JAVS_VENDOR.Controllers
{
    public class AdminVendorController : Controller
    {

        private readonly Vendor_Data vendorData;

        public AdminVendorController(Vendor_Data vdata)
        {
            vendorData = vdata;
        }


        [HttpGet]

        public IActionResult GetAllVendor()
        {

            var vendors = vendorData.GetAllVendors();
            return Ok(vendors);
        }


        [HttpGet("{id}")]


        public IActionResult GetVendorbyId(string id)
        {
            var vendor = vendorData.GetVendorById(id);
            if (vendor == null)
                return NotFound();

            return Ok(vendor);
        }


        [HttpPost]
        public IActionResult CreateVendor(VendorProfileDTO vendor)
        {
            vendorData.AddVendor(vendor);
            return CreatedAtAction(nameof(GetVendorbyId), new { id = vendor.UserId }, vendor);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVendor(string id, VendorProfileDTO vendor)
        {
            if (id != vendor.UserId)
            {
                return BadRequest();
            }
            vendorData.UpdateVendor(vendor);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVendor(string id)
        {
            vendorData.DeleteVendor(id);
            return NoContent();
        }
    }

}


