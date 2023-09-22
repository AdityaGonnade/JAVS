using System;

namespace UserDashboard.Data
{
	public class InventoryDBSettings
	{
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;

    }
}

