using System;
using JAVS_VENDOR.INVENTORY;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using JAVS_VENDOR.CART.CartDataAccess;
using JAVS_VENDOR.INVENTORY;
using JAVS_VENDOR.Repository;
using JWT_Token_Example.Inventory.InventoryEditDTO;
using JWT_Token_Example.Inventory.InventoryModels;
using JWT_Token_Example.Order.OrderModels;

namespace JAVS_VENDOR.ORDERS.OrderDataAccess
{
	public class OrderServices
	{

        private IInventoryRepo _inventoryDataRepo;

        private IOrderRepo repo;

        private ICartRepo _cartRepo;

        
        public OrderServices(IOrderRepo _repo,IInventoryRepo Irepo,ICartRepo cartRepo)
        {
             repo = _repo;
            _inventoryDataRepo = Irepo;
            _cartRepo = cartRepo;
        }
        public async Task UpdateDBOrderPlaced(EditReqDTO obj)
        {
            var filter =Builders<Product>.Filter.Eq("ProductName", obj.ProductName);
            
            var filter2 = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq("ProductName", obj.ProductName),
                Builders<Product>.Filter.ElemMatch("items", Builders<Items>.Filter.Eq("SellerId", obj.SellerId))
            );
            
            var pr = await _inventoryDataRepo.Fetchproduct(filter2);

            var proItem = await _inventoryDataRepo.Fetchproduct(filter);

            var Reqproduct = proItem.First();
            
            var Reqitem = await _inventoryDataRepo.GetRequiredItem(obj.ProductName, obj.SellerId);

            int total_quantity = Reqproduct.Quantity;

            var sellerList = Reqitem;
            
            sellerList.quantity  = sellerList.quantity-obj.quantity;
            
            total_quantity = total_quantity - obj.quantity;
            
            sellerList.Status = sellerList.quantity <= 0 ? "unavailable" : "available";
            
            var update =
                Builders<Product>.Update.PullFilter("items",
                    Builders<Items>.Filter.Eq("SellerId", obj.SellerId));

            var pushUpdate = Builders<Product>.Update.Push("items", sellerList);

            var totalUpdate = Builders<Product>.Update.Set("Quantity", total_quantity);
            
            string totalStatusUpdate= total_quantity>0?"available":"unavailable";
                        
            var update_status = Builders<Product>.Update.Set("Status", totalStatusUpdate);

            var combinedUpdate = Builders<Product>.Update.Combine(update,  totalUpdate,update_status);

            await _inventoryDataRepo.UpdateDB(filter, combinedUpdate);

            await _inventoryDataRepo.UpdateDB(filter, pushUpdate);
        }
        public async Task<List<Orders>> GetAllP()
        {

            return await repo.GetAllP();
        }
        public async Task<List<VendorOrdersDTO>> GetAllOrdersVendor(string vendorId)
        {
            var orders = await repo.GetOrdersVendor(vendorId);
        
            var result = orders.Select(order => new VendorOrdersDTO
            {
                OrderId = order.id,
                BillingAddressId = order.BillingAddressId,
                BuyerId = order.BuyerId,
                orderitems = order.orders
                    .Where(x => x.SellerId == vendorId && order.OrderStatus != "Cancelled")
                    .Select(x => new VendorOrderItems
                    { SellerId = x.SellerId,
                        Price = x.Price,
                        itemquantity = x.itemquantity
                    })
                    .ToList()
            }).ToList();
        
            return result;
        }

        
        public async Task<List<Orders>> GetOrdersPlacedBuyer(string id)
        {
            return await repo.GetOrdersPlacedBuyer(id);

        }
        
        // public async Task<Orders> CancelOrder(CancelOrderDTO obj)
        // {
        //
        //     foreach (var itemid in obj.ItemIds)
        //     {
        //         var filter = Builders<Orders>.Filter.And(
        //         Builders<Orders>.Filter.Eq("id", obj.OrderId),
        //         Builders<Orders>.Filter.Eq("orders.ItemId", itemid)
        //         );
        //         var update = Builders<Orders>.Update.Set("orders.$.OrderStatus", "Cancelled");
        //
        //         await ordersCollection.UpdateOneAsync(filter, update);
        //
        //
        //         var filt = Builders<Orders>.Filter.Eq("id", obj.OrderId);
        //         var pr = await ordersCollection.Find(filt).ToListAsync();
        //         var order = pr.First();
        //
        //         string req_productname = "";
        //         string req_sellerid="";
        //
        //         foreach(var items in order.orders)
        //         {
        //             if (itemid == items.ItemId)
        //             {
        //                 req_productname = items.ProductName;
        //                 req_sellerid = items.SellerId;
        //
        //                 break;
        //             }
        //
        //         }
        //
        //         var request = new EditReqDTO()
        //         {
        //
        //             ProductName = req_productname,
        //             SellerId = req_sellerid,
        //             quantity = 1,
        //         };
        //         await UpdateDBOrderCancelled(request);
        //
        //
        //
        //     }
        //
        //
        //
        //
        //     var filter2 = Builders<Orders>.Filter.Eq("id", obj.OrderId);
        //     var req = await ordersCollection.Find(filter2).ToListAsync();
        //     return req.First();
        // }

        
        
        //  public async Task UpdateDBOrderCancelled(EditReqDTO obj)
        // {//rechecked and optimised
        //     var filter =Builders<Product>.Filter.Eq("ProductName", obj.ProductName);
        //     
        //     var filter2 = Builders<Product>.Filter.And(
        //         Builders<Product>.Filter.Eq("ProductName", obj.ProductName),
        //         Builders<Product>.Filter.ElemMatch("items", Builders<Items>.Filter.Eq("SellerId", obj.SellerId))
        //     );
        //     
        //     // var pr = await dataAccess.productsCollection.Find(filter2).ToListAsync();
        //
        //     var proItem=await dataAccess.productsCollection.Find(filter).ToListAsync();
        //     
        //     // Product Reqproduct=new Product();
        //     var Reqproduct = proItem.First();
        //     // if (proItem.Count > 0)
        //     // {
        //     //    
        //     // }
        //
        //     // if (pr.Count() >= 0 && proItem.Count >= 0)
        //     // { // product exists with same seller id
        //
        //            
        //           
        //
        //     var pipeline = new BsonDocument[]
        //     {
        //         BsonDocument.Parse("{ $match: { ProductName: '" + obj.ProductName + "' } }"),
        //         BsonDocument.Parse("{ $unwind: '$items' }"),
        //         BsonDocument.Parse("{ $match: { 'items.SellerId': '" + obj.SellerId + "' } }"),
        //         BsonDocument.Parse("{ $replaceRoot: { newRoot: '$items' } }")
        //     };
        //
        //     var aggregation = dataAccess.productsCollection.Aggregate<BsonDocument>(pipeline);
        //
        //     var item = await aggregation.FirstOrDefaultAsync();
        //     Items Reqitem = new Items();
        //
        //     if (item != null)
        //     {
        //         Reqitem= BsonSerializer.Deserialize<Items>(item);
        //     }
        //
        //     
        //     int total_quantity = Reqproduct.Quantity;
        //    
        //
        //     var sellerList = Reqitem;
        //     // var sellerList = new Items()
        //     // { 
        //     //     ProductName = obj.ProductName,
        //     //     SellerId = obj.SellerId,
        //     // };
        //     // sellerList.quantity = obj.quantity == -1 ? Reqitem.quantity : obj.quantity;
        //     // sellerList.ImageUrl = obj.ImageUrl == "unknown" ? Reqitem.ImageUrl : obj.ImageUrl;
        //     // sellerList.Category = Reqproduct.Category;
        //     // sellerList.Discount = obj.Discount == -1 ? Reqitem.Discount : obj.Discount;
        //     // sellerList.Price = obj.Price == -1 ? Reqitem.Price : obj.Price;
        //     // sellerList.DateUploaded =Reqitem.DateUploaded;
        //     // sellerList.Descriptions = obj.Description == "unknown" ? Reqitem.Descriptions : obj.Description;
        //     // sellerList.Tags = Reqitem.Tags;
        //     sellerList.Status =  "available";
        //
        //     sellerList.quantity+=obj.quantity;
        //     total_quantity+=obj.quantity;
        //     var update =
        //         Builders<Product>.Update.PullFilter("items",
        //             Builders<Items>.Filter.Eq("SellerId", obj.SellerId));
        //
        //     var pushUpdate = Builders<Product>.Update.Push("items", sellerList);
        //
        //     var totalUpdate = Builders<Product>.Update.Set("Quantity", total_quantity);
        //
        //     var status_update = Builders<Product>.Update.Set("Status", "available");
        //
        //     var combinedUpdate = Builders<Product>.Update.Combine(update,  totalUpdate,status_update);
        //
        //     await dataAccess.productsCollection.UpdateOneAsync(filter, combinedUpdate);
        //
        //     await dataAccess.productsCollection.UpdateOneAsync(filter, pushUpdate);
        //
        //
        //    
        //
        // }

        // public async Task OrderCancelled(CancelOrderDTO obj)
        // {
        //     var ReqOrder = await repo.GetOrderById(obj.OrderId);
        //
        //     foreach (var items in ReqOrder.orders)
        //     {
        //         var i = new Items()
        //         {
        //             SellerId = items.SellerId,
        //             quantity = items.itemquantity,
        //             ProductName = items.ProductName
        //         };
        //
        //         await dataAccess.AddItem(i);
        //     }
        // }
         public async Task<Orders> PlaceOrderBuyer(OrdersDTO obj)
         {
             var orderobj = new Orders()
             {
                 BillingAddressId = obj.BillingAddressId,
                 BuyerId = obj.BuyerId,
                 orders=new List<OrderItems>(),
                 OrderStatus = "Order Placed"

             };

             foreach (var item in obj.orders)
             {
                 var Reqitem=await _inventoryDataRepo.GetRequiredItem(item.ProductName, item.SellerId);
                 if (item.itemquantity > Reqitem.quantity)
                 {
                     return null;
                 }
                 
             }

             int totalqty = 0;
             
             long totalamount = 0;
             
             foreach (var item in obj.orders)
             {
                 var Reqitem=await _inventoryDataRepo.GetRequiredItem(item.ProductName, item.SellerId);
                 
                 item.Price = Reqitem.Price;
                 
                 totalqty += item.itemquantity;
                 
                 totalamount += (item.itemquantity * item.Price);
                 
                 var orderitem = new OrderItems()
                 {
                     ProductName = item.ProductName,
                     SellerId = item.SellerId,
                     Price = item.Price,
                     itemquantity = item.itemquantity,
                     dateOfArrival = DateTime.Today,
                    
                 };
                 orderobj.orders.Add(orderitem);
                 
                 var request = new EditReqDTO()
                 {
                     ProductName = item.ProductName,
                     SellerId = item.SellerId,
                     quantity = item.itemquantity,
        
                 };
        
                 await UpdateDBOrderPlaced(request);

             }
             orderobj.TotalAmount = totalamount;
             
             orderobj.TotalQuantity = totalqty;
             
             await _cartRepo.deleteCart(obj.BuyerId);
             
             await repo.Insert(orderobj);
             
             return orderobj;


         }
    }
    
    
}

