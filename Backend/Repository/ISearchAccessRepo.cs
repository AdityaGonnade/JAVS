using JWT_Token_Example.Inventory.InventoryModels;
using MongoDB.Driver;

namespace JWT_Token_Example.Repository;

public interface ISearchAccessRepo
{
    public Task<List<Product>> GetIventoryData();
}