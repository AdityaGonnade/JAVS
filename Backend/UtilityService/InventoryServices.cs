using System;
using System.Runtime.Intrinsics.X86;
using MongoDB.Bson;
using MongoDB.Driver;

using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.Servers;
using JAVS_VENDOR.Repository;
using JWT_Token_Example.Inventory.InventoryDeleteDTO;
using JWT_Token_Example.Inventory.InventoryEditDTO;
using JWT_Token_Example.Inventory.InventoryModels;
using MongoDB.Bson.Serialization;

using ZstdSharp.Unsafe;

namespace JAVS_VENDOR.INVENTORY
{
	public class InventoryServices
	{ 
        private readonly IInventoryRepo _repo;
        
        public InventoryServices(IInventoryRepo repo)
        {
            _repo = repo;
        
        }
        public async Task<List<Product>> GetAllP()
        { 

            var results = await _repo.FetchAllProducts();
            return results;
        }
        public async Task<List<Items>> GetAllProducts(string vendorId)
        { 
            var filter = Builders<Product>.Filter.ElemMatch(p => p.items, i => i.SellerId == vendorId);
            var projection = Builders<Product>.Projection.Include("Items");

            var products = await _repo.FetchAllProductsVendor(filter,projection);

            var result = new List<Items>();
    
            foreach (var product in products)
            {
                result.AddRange(product.items.Where(item => item.SellerId == vendorId));
            }
    
            return result;
        }

        public async Task AddItem(Items p)
        {
            var filter =Builders<Product>.Filter.Eq("ProductName", p.ProductName);
            
            var filter2 = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq("ProductName", p.ProductName),
                Builders<Product>.Filter.ElemMatch("items", Builders<Items>.Filter.Eq("SellerId", p.SellerId))
            );

            var pr = await _repo.Fetchproduct(filter2);

            var proItem=await _repo.Fetchproduct(filter);
            
            Product Reqproduct=new Product();
            
            if (proItem.Count > 0)
            {
                Reqproduct = proItem.First();
            }
                
            if (pr.Count() <= 0 && proItem.Count<=0 )
            {
                Product pro = new Product()
                {
                    ProductName = p.ProductName,
                    Category = p.Category,
                    items = new List<Items>(
            
                        ),
            
                    Quantity = p.quantity,
                    Status = "Available"
            
            
                };
            
                pro.items.Add(p);
                
                await _repo.InsertOneDB(pro);
            }
            
            else if(pr.Count() <= 0)
            {  
            
                Reqproduct.Quantity += p.quantity;
               
                p.Status = "Available";
                
                Reqproduct.items.Add(p);

                await _repo.ReplaceOneDB(filter, Reqproduct);

            }

            else
            {
                var update = Builders<Product>.Update.Inc("items.$.quantity",p.quantity );

                var update_total = Builders<Product>.Update.Inc("Quantity", p.quantity);
                
                var update_Status = Builders<Product>.Update.Set("Status", "available");

                // await productsCollection.UpdateOneAsync(filter, update_total);

                var combinedUpdate = Builders<Product>.Update.Combine(update_total, update_Status);
                await _repo.UpdateDB(filter, combinedUpdate);

                // await productsCollection.UpdateOneAsync(filter2, update);
                await _repo.UpdateDB(filter2, update);
            }
           

        }
        public async Task DeleteProduct(DeleteReqDTO obj)
        { 
            var filter = Builders<Product>.Filter.Eq("ProductName", obj.ProductName);
            
            var pr = await _repo.Fetchproduct(filter);
            
            int new_prod_quantity = 0;

            if (pr.Count() > 0)
            {
                var product = pr.First();
                new_prod_quantity = product.Quantity - obj.prev_quantity+obj.new_quantity;

                var update_quantity = Builders<Product>.Update.Set("Quantity", new_prod_quantity);

                var filter2 = Builders<Product>.Filter.And(
                    Builders<Product>.Filter.Eq("ProductName", obj.ProductName),
                    Builders<Product>.Filter.Eq("items.SellerId", obj.SellerId)
                );
                
                var update = Builders<Product>.Update.Set("items.$.quantity", obj.new_quantity);

                var status_updated = obj.new_quantity <= 0 ? "unavailable" : "available";
                
                var update_stat = Builders<Product>.Update.Set("items.$.Status", status_updated);

                var combinedUpdate = Builders<Product>.Update.Combine(update_stat, update,update_quantity);

                await _repo.UpdateDB(filter2, combinedUpdate);
                
                if (new_prod_quantity <= 0)
                {
                    var statusupdate = Builders<Product>.Update.Set("Status", "unavailable");
                  
                   await _repo.UpdateDB(filter, statusupdate);

                }

            }

        }
        
        
           public async Task EditItem(EditReqDTO obj)
        {
            var filter =Builders<Product>.Filter.Eq("ProductName", obj.ProductName);
            
            var filter2 = Builders<Product>.Filter.And(
                Builders<Product>.Filter.Eq("ProductName", obj.ProductName),
                Builders<Product>.Filter.ElemMatch("items", Builders<Items>.Filter.Eq("SellerId", obj.SellerId))
            );

            var pr = await _repo.Fetchproduct(filter2);
            
            var proItem = await _repo.Fetchproduct(filter);
            
            Product Reqproduct=new Product();
            if (proItem.Count > 0)
            {
                Reqproduct = proItem.First();
            }

            if (pr.Count() >= 0 && proItem.Count >= 0)
            { 

            var Reqitem = await _repo.GetRequiredItem(obj.ProductName, obj.SellerId);

            int total_quantity = Reqproduct.Quantity;
            
            total_quantity -= Reqitem.quantity;
            
            total_quantity += obj.quantity;
            
            var sellerList = new Items()
            { 
                ProductName = obj.ProductName,
                SellerId = obj.SellerId,
            };
            
            sellerList.quantity = obj.quantity == -1 ? Reqitem.quantity : obj.quantity;
            
            sellerList.ImageUrl = obj.ImageUrl == "unknown" ? Reqitem.ImageUrl : obj.ImageUrl;
            
            sellerList.Category = Reqproduct.Category;
            
            sellerList.Discount = obj.Discount == -1 ? Reqitem.Discount : obj.Discount;
            
            sellerList.Price = obj.Price == -1 ? Reqitem.Price : obj.Price;
            
            sellerList.DateUploaded =Reqitem.DateUploaded;
            
            sellerList.Descriptions = obj.Description == "unknown" ? Reqitem.Descriptions : obj.Description;
            
            sellerList.Tags = Reqitem.Tags;
            
            sellerList.Status = obj.quantity <= 0 ? "unavailable" : "available";
            var update =
                Builders<Product>.Update.PullFilter("items",
                    Builders<Items>.Filter.Eq("SellerId", obj.SellerId));

            var pushUpdate = Builders<Product>.Update.Push("items", sellerList);

            var totalUpdate = Builders<Product>.Update.Set("Quantity", total_quantity);

            string totalStatusUpdate= total_quantity>0?"available":"unavailable";
            
            var update_status = Builders<Product>.Update.Set("Status", totalStatusUpdate);

            var combinedUpdate = Builders<Product>.Update.Combine(update,  totalUpdate,update_status);

            await _repo.UpdateDB(filter, combinedUpdate);

            await _repo.UpdateDB(filter, pushUpdate);

            }

        }
    }
}

