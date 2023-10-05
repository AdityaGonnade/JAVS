

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using JAVS_VENDOR.INVENTORY;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Servers;

using JAVS_VENDOR.Repository;
using JWT_Token_Example.Carts;
using JWT_Token_Example.Inventory.InventoryModels;
using JWT_Token_Example.Models.DTO;

namespace JAVS_VENDOR.CART.CartDataAccess
{

    public class CartServices
    {
        private readonly IInventoryRepo dataAccess;

        private readonly ICartRepo repo;
        // public readonly IMongoCollection<Cart> cartCollection;


        public CartServices(IInventoryRepo services,ICartRepo Irepo)
        {
            dataAccess = services;
            repo = Irepo;
        }


        public async Task<List<Cart>> GetAllCartItems(string BuyerId)
        {//done

            return await repo.GetAllCart(BuyerId);
        }
        
        public async Task<Cart> AddtoCart(GetProductDto getProductDto)
        {// done,checked and optimised
            var filter = Builders<Product>.Filter.Eq("ProductName", getProductDto.ProductName);
            
            var Reqitem = await dataAccess.GetRequiredItem(getProductDto.ProductName, getProductDto.SellerId);

            int currentitemprice = Reqitem.Price;
            
            string currentitemimage=Reqitem.ImageUrl;

            var fil = Builders<Cart>.Filter.Eq("BuyerId", getProductDto.BuyerId);

            var currentItem = await repo.GetAllCart(getProductDto.BuyerId);
            
            Cart request=new Cart();

            if (currentItem.Count() > 0)
            { var  car = currentItem.First();
              
                request.Items = car.Items;
                
                request.BuyerId = car.BuyerId;
                foreach (var items in car.Items)
                {
                    if (getProductDto.ProductName == items.ProductName && getProductDto.SellerId == items.SellerId)
                    {
                        EditCartDTO obj = new EditCartDTO()
                        {
                            BuyerId = getProductDto.BuyerId,
                            item = new EditCartItemsDTO()
                            {
                                SellerId = getProductDto.SellerId,
                                Quantity = getProductDto.quantity+items.Quantity,
                                ProductName = getProductDto.ProductName
                            }
                        };
                        return await EditItemsCart(obj);
                    }
                }
                CartItems cartItem = new CartItems()
                {
                    SellerId = getProductDto.BuyerId,
                    Quantity = getProductDto.quantity,
                    ProductName = getProductDto.ProductName,
                    Price = currentitemprice,
                    Image = currentitemimage
                };
                
                var pushUpdate = Builders<Cart>.Update.Push("Items", cartItem);

                await repo.UpdateCart(fil, pushUpdate);
                
                return request;

            }
            
            else
            { 
                CartItems cartItem = new CartItems()
                {
                    SellerId = getProductDto.SellerId,
                    Quantity = getProductDto.quantity,
                    ProductName = getProductDto.ProductName,
                    Price = currentitemprice,
                    Image = currentitemimage
                };
                
                Cart cartobj = new Cart()
                {
                    BuyerId = getProductDto.BuyerId,
                    Items = new List<CartItems>()
                };
             
                cartobj.Items.Add(cartItem);
            
                await repo.InsertIntoCart(cartobj);

                return cartobj;
            }

            return null;
        }

        public async Task RemoveFromcart(string BuyerId, CartItems item)
        {   
            var filter = Builders<Cart>.Filter.Eq("BuyerId", BuyerId);
            
            var currentItem = await repo.GetAllCart(BuyerId);
            
            var req = currentItem.First();
            
            foreach (var x in req.Items)
            {
                int index;
                if (x == item)
                {
                    req.Items.Remove(x);
                    break;
                }
            }

            var final = req.Items;

            var finalCart = new Cart()
            {
                Id = req.Id,
                BuyerId = req.BuyerId,
                Items = final
            };
            
            var update = Builders<Cart>.Update.Set("Items", final);
            
            await repo.UpdateCart(filter, update);


        }

        public async Task DeleteCart(string BuyerId)
        {
            await repo.deleteCart(BuyerId);

        }
       
        public async Task<Cart> EditItemsCart(EditCartDTO obj)
        {
            var filter = Builders<Cart>.Filter.Eq("BuyerId", obj.BuyerId);
            
            var req = await repo.GetAllCart(obj.BuyerId);
            
            var overallcart = req.First();

            var finalCart = new Cart();

            finalCart.BuyerId = overallcart.BuyerId;
            
            finalCart.Items = new List<CartItems>();
            
            foreach (var x in overallcart.Items)
            {
                if (x.ProductName != obj.item.ProductName || x.SellerId != obj.item.SellerId)
                {
                    finalCart.Items.Add(x);
                }
               
                else if (x.ProductName == obj.item.ProductName && x.SellerId == obj.item.SellerId)
                {

                    var car = new CartItems()
                    {
                        SellerId = x.SellerId,
                        ProductName = x.ProductName,
                        Image = x.Image,
                        Quantity = obj.item.Quantity,
                        Price = x.Price,
                        qtyAvailableSeller = x.qtyAvailableSeller
                    };
                    
                    finalCart.Items.Add(car);
                }
                
            }

            var update = Builders<Cart>.Update.Set("Items", finalCart.Items);
            
            await repo.UpdateCart(filter,update);

            return finalCart;

        }

    }
}