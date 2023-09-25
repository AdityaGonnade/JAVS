using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using JAVS_VENDOR.VENDORPROFILE_SQL_DATA;
using JAVS_VENDOR.VendorProfile.VendorProfileModels.VendorProfileDTO;
using JAVS_VENDOR.VendorProfileModels.VendorProfileModels.VendorProfileDomain;

namespace JAVS_VENDOR.PROFILE
{
	public class Vendor_Data
	{

		private readonly VendorProfileDBcontext dbcontext;


		public Vendor_Data(VendorProfileDBcontext context)
		{
			dbcontext = context;
		}


        // get all vendors for admin
        public IEnumerable<VendorProfileDTO> GetAllVendors()
        {
            var list = dbcontext.vendors.ToList();
            List<VendorProfileDTO> res=new List<VendorProfileDTO>();

            List<User> userlist;
            foreach(Vendor v in list)
            {
                var userdata = dbcontext.users.Find(v.UserId);

                VendorProfileDTO vendor = new VendorProfileDTO()
                {
                    UserId = userdata.UserId,
                    emailId = userdata.emailId,
                    name = userdata.name,
                    MobileNo = userdata.MobileNo,
                    AccountCreated = userdata.AccountCreated,
                    LastLogin = userdata.LastLogin,
                    UserType = userdata.UserType,
                    Password=null,
                    GST = v.GST,
                    PAN = v.PAN,
                    BankAccountNo = v.BankAccountNo
                };

                res.Add(vendor);

            }

            return res;
           

        }

        // get vendor by given id
        public VendorProfileDTO GetVendorById(string id)
        {
            var userdata= dbcontext.users.Find(id);
            var vendata = dbcontext.vendors.Find(id);


          
            VendorProfileDTO vendor = new VendorProfileDTO()
            {
                UserId = userdata.UserId,
                emailId = userdata.emailId,
                name = userdata.name,
                MobileNo = userdata.MobileNo,
                AccountCreated = userdata.AccountCreated,
                LastLogin = userdata.LastLogin,
                UserType = userdata.UserType,
                Password=null,
                GST = vendata.GST,
                PAN = vendata.PAN,
                BankAccountNo = vendata.BankAccountNo
            };

            return vendor;
        }

        // add vendors by admin or through login
        public void AddVendor(VendorProfileDTO vendor)
        {
            vendor.AccountCreated = DateTime.Now;

            Vendor ven = new Vendor()
            {
                GST = vendor.GST,
                PAN = vendor.PAN,
                BankAccountNo = vendor.BankAccountNo
            };


            User use = new User()
            {
                UserId = vendor.UserId,
                emailId = vendor.emailId,
                name = vendor.name,
                MobileNo = vendor.MobileNo,
                AccountCreated = DateTime.Now,
                Password=vendor.Password,
                LastLogin = DateTime.Now,
                UserType = vendor.UserType,
            };
            dbcontext.vendors.Add(ven);
            dbcontext.users.Add(use);
            dbcontext.SaveChanges();
        }

        // update vendor by admin/ vendor 
        public async void UpdateVendor(VendorProfileDTO vendor)
        {

            var x = await dbcontext.users.FirstOrDefaultAsync(x => x.UserId == vendor.UserId);

            var y = await dbcontext.vendors.FirstOrDefaultAsync(y => y.UserId == vendor.UserId);

            if (x == null || y == null)
                return;
            y.GST = vendor.GST;
                y.PAN = vendor.PAN;
            y.BankAccountNo = vendor.BankAccountNo;




            x.UserId = vendor.UserId;
            x.emailId = vendor.emailId;
            x.name = vendor.name;
            x.Password = vendor.Password;
            x.MobileNo = vendor.MobileNo;
            x.AccountCreated = DateTime.Now;
            x.LastLogin = vendor.LastLogin;
            x.UserType = vendor.UserType;
            

       
           await dbcontext.SaveChangesAsync();
        }



        // delete vendor by admin
        public void DeleteVendor(string id)
        {
            var vendor = dbcontext.vendors.Find(id);
            var use = dbcontext.users.Find(id);
            if (vendor != null)
            {
                dbcontext.vendors.Remove(vendor);
                dbcontext.users.Remove(use);
                dbcontext.SaveChanges();
            }

           
        }


    }
}

