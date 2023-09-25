using System;
namespace JAVS_VENDOR.VendorProfile.VendorProfileModels.VendorProfileDTO
{
	public class VendorProfileDTO
	{


        public string UserId { get; set; }

        public string emailId { get; set; }

        public string name { get; set; }

        public string Password { get; set; }

        public int MobileNo { get; set; }

        public DateTime AccountCreated { get; set; }

        public DateTime LastLogin { get; set; }

        public string UserType { get; set; }
        public string GST { get; set; }

        public string PAN { get; set; }


        public string BankAccountNo { get; set; }
    }
}

