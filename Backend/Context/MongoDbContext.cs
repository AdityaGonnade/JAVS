namespace JWT_Token_Example.Context;

public interface MongoDbContext
{
    public string ConnectionURI { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }

    public string CollectionName2 { get; set; }
}