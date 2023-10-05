using JWT_Token_Example.Inventory.InventorySearchAccess;

namespace JWT_Token_Example.UtilityService;

public interface ISearchAccess
{
    public Task<List<InventorySearchResponseDTO>> SearchProduct(string input);
    public Task<InventoryProductResponseDTO> GetProductByProductNameAndSellerId(string productName, string sellerId);
    public Task<List<InventoryProductResponseDTO>> GetProductBycategory(string category);

    
}