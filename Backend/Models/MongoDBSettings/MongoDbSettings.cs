namespace JWT_Token_Example.Models.MongoDBSettings;

public class MongoDbSettings
{
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string InventoryCollectionName { get; set; } = null!;
}