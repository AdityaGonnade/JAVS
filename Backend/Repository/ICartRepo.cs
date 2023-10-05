using JAVS_VENDOR.CART;
using JWT_Token_Example.Carts;
using MongoDB.Driver;

namespace JAVS_VENDOR.Repository;


public interface ICartRepo
{
    public Task<List<Cart>> GetAllCart(string BuyerId);

    public Task UpdateCart(FilterDefinition<Cart> filter, UpdateDefinition<Cart> update);

    public Task InsertIntoCart(Cart obj);

    public Task deleteCart(string BuyerId);
    
    
}