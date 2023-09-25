using System;
using Microsoft.EntityFrameworkCore;
using JAVS_VENDOR.PROFILE;
using  JAVS_VENDOR.VendorProfileModels.VendorProfileModels.VendorProfileDomain;


namespace JAVS_VENDOR.VENDORPROFILE_SQL_DATA
{
    public class VendorProfileDBcontext : DbContext
    {
        public VendorProfileDBcontext()
        {
        }

        public DbSet<Vendor> vendors { get; set; }

        public DbSet<User> users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vendor>().HasData(vendors);
            modelBuilder.Entity<User>().HasData(users);

        }
        
    }
}
