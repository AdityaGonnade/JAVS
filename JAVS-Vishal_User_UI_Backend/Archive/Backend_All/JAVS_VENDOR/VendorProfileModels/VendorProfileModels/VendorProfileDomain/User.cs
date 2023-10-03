using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JAVS_VENDOR.PROFILE
{
	public class User
	{
      

        public string UserId { get; set; }

        public  string emailId { get; set; }

		public string name { get; set; }

		public string Password { get; set; }

		public int MobileNo { get; set; }

		public DateTime AccountCreated { get; set; }

        public DateTime LastLogin { get; set; }

        public string UserType { get; set; }




    }
}

