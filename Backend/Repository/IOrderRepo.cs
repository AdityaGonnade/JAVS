
using JWT_Token_Example.Order.OrderModels;

namespace JAVS_VENDOR.Repository;


public interface IOrderRepo
{
    public Task<List<Orders>> GetAllP();

    public Task<List<Orders>> GetOrdersVendor(string vendorid);

    public Task<List<Orders>> GetOrdersPlacedBuyer(string id);

    public Task Insert(Orders obj);

    public Task<Orders> GetOrderById(string id);


}