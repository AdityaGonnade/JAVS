using JAVS_VENDOR.INVENTORY;
using MongoDB.Driver;
using JWT_Token_Example.Inventory.InventoryModels;

namespace JAVS_VENDOR.Repository;

public interface IInventoryRepo
{
   public Task<List<Product>> FetchAllProducts();
   
   public Task<List<Product>> FetchAllProductsVendor(FilterDefinition<Product> filter,ProjectionDefinition<Product> projection);

   public Task<List<Product>> Fetchproduct(FilterDefinition<Product> filter);

   public Task UpdateDB(FilterDefinition<Product> filter, UpdateDefinition<Product> update);

   public Task<Items> GetRequiredItem(string productname, string sellerid);

   public Task InsertOneDB(Product p);

   public Task ReplaceOneDB(FilterDefinition<Product> filter, Product p);



}