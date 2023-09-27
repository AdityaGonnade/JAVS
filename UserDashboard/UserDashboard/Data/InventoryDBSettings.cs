using System;

//Configuration file for Mongo DB
namespace UserDashboard.Data
{
	public class InventoryDBSettings
	{
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string InventoryCollectionName { get; set; } = null!;

        public string CartCollectionName { get; set; } = null;

	}
}

